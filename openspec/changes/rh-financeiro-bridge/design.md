# Design — rh-financeiro-bridge

## Pipeline de geração

```
                   FolhaMensal.Fechada
                          │
                          ▼
              ┌────────────────────────┐
              │ GeradorLancamentosFin. │
              └──────┬─────────────────┘
                     │
        ┌────────────┼─────────────┬─────────────┐
        ▼            ▼             ▼             ▼
   ┌─────────┐ ┌──────────┐ ┌──────────┐ ┌────────────┐
   │ N líq.  │ │ 1 GPS    │ │ 1 DARF   │ │ 1 GRF      │
   │ Funcion.│ │ (INSS)   │ │ (IRRF)   │ │ (FGTS)     │
   └────┬────┘ └────┬─────┘ └────┬─────┘ └─────┬──────┘
        │           │            │             │
        ▼           ▼            ▼             ▼
   ContaPagar  ContaPagar    ContaPagar    ContaPagar
   (1/func.)   (agregada)    (agregada)    (agregada)
        │           │            │             │
        └───────────┴──── tudo associado a ────┴────► LancamentoFinanceiroFolha
                                                      (tabela ponte de rastreio)
```

## Configuração de alíquotas por empresa

```sql
ALTER TABLE empresas
  ADD COLUMN rat_pct DECIMAL(3,2) DEFAULT 2.0,      -- 1/2/3% conforme grau de risco
  ADD COLUMN fap_pct DECIMAL(4,3) DEFAULT 1.0,       -- 0.5 a 2.0 — multiplicador RAT
  ADD COLUMN terceiros_pct DECIMAL(5,2) DEFAULT 5.8, -- INCRA, SESI, SENAI, etc.
  ADD COLUMN cnae_codigo CHAR(7);
```

RAT efetivo = RAT × FAP. Salvado dessa forma para auditoria.

## Vencimentos brasileiros

```csharp
public static class VencimentosFolha
{
    // GPS (Lei 8.212 art. 30)
    public static DateOnly Gps(DateOnly competencia) => ProximoDiaUtil(new(comp.Year, comp.Month, 20).AddMonths(1));

    // DARF IRRF (atualmente: 2º dia útil após o evento — folha mensal: dia 20 mes+1)
    public static DateOnly DarfIrrf(DateOnly competencia) => ProximoDiaUtil(new(comp.Year, comp.Month, 20).AddMonths(1));

    // GRF FGTS (Lei 8.036 art. 15)
    public static DateOnly GrfFgts(DateOnly competencia) => ProximoDiaUtil(new(comp.Year, comp.Month, 7).AddMonths(1));

    // GRRF rescisão
    public static DateOnly GrrfFgts(DateOnly dataRescisao) => ProximoDiaUtil(dataRescisao.AddDays(10));

    // Folha líquida — default dia 5 mes+1 (configurável por empresa)
    public static DateOnly Liquido(DateOnly competencia, int diaPagamento)
        => ProximoDiaUtil(new(comp.Year, comp.Month, diaPagamento).AddMonths(1));

    private static DateOnly ProximoDiaUtil(DateOnly d) { /* skipFeriados+finsDeSem */ }
}
```

## Geração idempotente

```csharp
public async Task GerarAsync(Guid folhaId)
{
    // Carrega tudo
    var folha = await _repo.ObterAsync(folhaId);
    var holerites = await _holeriteRepo.ListarAsync(folhaId);
    var lancamentosExistentes = await _lancRepo.ListarPorFolhaAsync(folhaId);

    // Se já tem lançamentos e folha não foi reaberta → noop
    if (lancamentosExistentes.Any() && folha.Status == StatusFolha.Fechada)
        return;  // idempotente

    // Cancela existentes (em caso de reabertura)
    foreach (var l in lancamentosExistentes)
        await _contaPagarService.CancelarAsync(l.ContaPagarId);

    using var trx = await _db.BeginTransactionAsync();

    foreach (var h in holerites)
    {
        var contaPagar = await _contaPagarService.CriarAsync(new {
            Beneficiario = h.Funcionario.NomeCompleto,
            Cpf = h.Funcionario.Cpf,
            ContaBancaria = h.Funcionario.ContaBancariaJson,
            Valor = h.ValorLiquido,
            Vencimento = VencimentosFolha.Liquido(folha.Competencia, empresa.DiaPagamento),
            Descricao = $"Folha {folha.Competencia} — {h.Funcionario.NomeCompleto}",
            OrigemFolhaId = folha.Id,
            CentroCustoId = h.Funcionario.CentroDeCustoId
        });
        await _lancRepo.CriarAsync(LancamentoFinanceiroFolha.LiquidoFuncionario(folha.Id, h.Id, contaPagar.Id));
    }

    // GPS agregado
    var inssEmpregado = holerites.Sum(h => h.RubricaValor("100-INSS-DESC"));
    var baseInss = holerites.Sum(h => h.BaseInss);
    var inssPatronal = baseInss * 0.20m;
    var rat = baseInss * (empresa.RatPct / 100m) * empresa.FapPct;
    var terceiros = baseInss * (empresa.TerceirosPct / 100m);
    var totalGps = inssEmpregado + inssPatronal + rat + terceiros;
    var gps = await _contaPagarService.CriarAsync(new {
        Beneficiario = "GPS Receita Federal",
        Valor = totalGps,
        Vencimento = VencimentosFolha.Gps(folha.Competencia),
        Descricao = $"GPS competência {folha.Competencia}",
        ...
    });
    await _lancRepo.CriarAsync(LancamentoFinanceiroFolha.GpsInss(folha.Id, gps.Id, ...));

    // DARF e GRF análogos
    ...

    await trx.CommitAsync();
}
```

## Reverse sync

```
Módulo Financeiro: PagarContaPagarCommandHandler
  Após gravar pagamento:
    await _mediator.Publish(new ContaPagarFoiPagaEvent(contaPagar));

RH: ContaPagarFoiPagaEventHandler
  if (contaPagar.OrigemFolhaId.HasValue):
    var lanc = await _lancRepo.PorContaPagarAsync(contaPagar.Id);
    lanc.Status = StatusLancamento.Pago;
    await _lancRepo.AtualizarAsync(lanc);
    if (lanc.Tipo == LiquidoFuncionario):
      await _notificacao.NotificarFuncionarioAsync(lanc.ReferenciaId, "Holerite pago em ...");
```

## Tradeoffs

### Por que não criar ContaPagar diretamente em vez de ter LancamentoFinanceiroFolha?

Tabela ponte permite:
- Saber qual ContaPagar veio de qual folha (sem mexer no schema do Financeiro);
- Cancelar em bloco quando folha reabre;
- Relatório de conciliação.

### Por que agregar GPS/DARF/GRF em vez de 1 conta por funcionário?

São impostos pagos pela empresa, não pelo funcionário. Receita Federal recebe 1 guia agregada por competência por empresa.

### Pagamento de líquido — 1 conta por funcionário ou batelada?

1 por funcionário (a tela do Financeiro filtra por origem_folha e dá ação "Pagar selecionados"). Banco vai receber via CNAB/SISPAG (fora escopo W10).

## Test strategy

- Unit: geradores agregados (GPS, DARF, GRF).
- Unit: VencimentosFolha por mês × feriado.
- Integration: folha 10 funcs → 10 + 3 ContaPagar criadas.
- Integration: reabrir folha cancela + recria.
- Integration: pagar ContaPagar atualiza lançamento + notifica funcionário.
