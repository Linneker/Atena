# Design — rh-folha-engine

## Visão do pipeline

```
                          POST /folha/{competencia}/abrir
                                       │
                                       ▼
                              ┌────────────────┐
                              │  FolhaMensal   │
                              │   .Aberta      │
                              └───────┬────────┘
                                      │ POST /folha/{id}/calcular
                                      ▼
            ┌─────────────────────────────────────────────────┐
            │  Enfileira N CalcularFolhaMessage (1/funcio.)   │
            └─────────────────────┬───────────────────────────┘
                                  │ RabbitMQ paralelo (workers x4)
                                  ▼
    ┌─────────────────────────────────────────────────────────┐
    │  CalculoFolhaWorker (por funcionário)                   │
    │   ├── PreparaContexto                                   │
    │   ├── ResolveRubricasDeOficio (CLT base)                │
    │   ├── AplicaRubricasCustomTenant (DSL via W5)           │
    │   ├── ConsolidaTotais                                   │
    │   ├── Persiste HoleriteFuncionario                      │
    │   └── (não gera PDF agora — separado)                   │
    └─────────────────────────┬───────────────────────────────┘
                              │ todos concluídos
                              ▼
                    FolhaMensal.Calculada
                              │
                  RH revisa via /holerites
                  pode recalcular indiv. ou ajustar rubrica
                              │
                       /folha/{id}/conferir
                              ▼
                    FolhaMensal.Conferida
                              │
                        /folha/{id}/fechar
                              ▼
                     FolhaMensal.Fechada ──► dispara:
                                                ├── geração PDFs em massa (worker)
                                                ├── notificação por e-mail
                                                ├── pendência para W10 (Financeiro)
                                                └── pendência para W14 (eSocial S-1200/1210)
```

## Algoritmo de cálculo (1 funcionário, 1 competência)

```csharp
async Task<HoleriteFuncionario> Calcular(Guid funcionarioId, string competencia)
{
    var ctx = await PrepararContextoAsync(funcionarioId, competencia);

    // ETAPA 1 — Rubricas de ofício (CLT base)
    ctx.Add(rubrica: "001-SAL-BASE", valor: ctx.SalarioBaseProporcional());
    ctx.Add(rubrica: "020-HE-50",    valor: ctx.HoraExtra50());
    ctx.Add(rubrica: "021-HE-100",   valor: ctx.HoraExtra100());
    ctx.Add(rubrica: "030-ADIC-NOT", valor: ctx.AdicionalNoturno());
    ctx.Add(rubrica: "040-PERIC",    valor: ctx.Periculosidade());
    ctx.Add(rubrica: "041-INSALUB",  valor: ctx.Insalubridade());
    ctx.Add(rubrica: "050-DSR-HE",   valor: ctx.DsrSobreHe(), condicional: ctx.Param.PagaDsrSobreHe);
    ctx.Add(rubrica: "060-SAL-FAM",  valor: ctx.SalarioFamilia());
    ctx.Add(rubrica: "070-FALTAS",   valor: -ctx.DescontoFaltas());     // negativo
    ctx.Add(rubrica: "080-ATRASOS",  valor: -ctx.DescontoAtrasos());

    // ETAPA 2 — Benefícios (do cadastro do funcionário)
    foreach (var ben in ctx.Beneficios)
        ctx.Add(rubrica: ben.Codigo, valor: ben.Valor, baseCalc: ben.BaseInfo);

    // ETAPA 3 — Rubricas custom do tenant (DSL via W5)
    var ordemTop = TopologicalSort(ctx.RubricasCustom);
    foreach (var rub in ordemTop)
    {
        var resultado = await _evaluator.AvaliarAsync(rub.FormulaDsl, ctx.AsRubricaContexto());
        ctx.Add(rubrica: rub.Codigo, valor: resultado);
    }

    // ETAPA 4 — Bases para descontos legais
    ctx.BaseInss = SomaPorIncidencia(ctx.Rubricas, r => r.IncideInss);
    ctx.BaseIrrf = ctx.BaseInss - ctx.InssCalculadoSubtotal() - (ctx.DependentesIrrf * 189.59m) - ctx.PensaoAlim;
    ctx.BaseFgts = SomaPorIncidencia(ctx.Rubricas, r => r.IncideFgts);
    ctx.BaseDsr  = SomaPorIncidencia(ctx.Rubricas, r => r.IncideDsr);

    // ETAPA 5 — Descontos legais (após termos as bases)
    ctx.Add(rubrica: "100-INSS-DESC",  valor: -ctx.AplicaTabelaInss());
    ctx.Add(rubrica: "110-IRRF-DESC",  valor: -ctx.AplicaTabelaIrrf());
    ctx.Add(rubrica: "120-VT-DESC",    valor: -ctx.DescontoVT());

    // ETAPA 6 — Informativos (não somam no líquido)
    ctx.Add(rubrica: "900-FGTS-INFO",  valor: ctx.BaseFgts * 0.08m, tipo: Informativa);

    // ETAPA 7 — Consolidação
    var totalProv = ctx.Rubricas.Where(r => r.Tipo == Provento && r.Valor > 0).Sum(r => r.Valor);
    var totalDesc = ctx.Rubricas.Where(r => r.Tipo == Desconto || r.Valor < 0).Sum(r => Math.Abs(r.Valor));
    var liquido = totalProv - totalDesc;

    return new HoleriteFuncionario
    {
        FolhaId = ctx.FolhaId,
        FuncionarioId = funcionarioId,
        RubricasCalculadas = ctx.Rubricas.ToList(),
        TotalProventos = totalProv,
        TotalDescontos = totalDesc,
        ValorLiquido = liquido,
        BaseInss = ctx.BaseInss,
        BaseIrrf = ctx.BaseIrrf,
        BaseFgts = ctx.BaseFgts,
        BaseDsr = ctx.BaseDsr
    };
}
```

## Helpers do contexto

`ContextoFuncionarioFolha` é classe puramente dados, instanciada uma vez por funcionário, com métodos read-only de cálculo CLT (HoraExtra50, SalarioFamilia, etc.). **Não tem estado mutável** além da coleção de rubricas calculadas.

```csharp
public sealed class ContextoFuncionarioFolha
{
    public Guid FolhaId { get; init; }
    public Funcionario Funcionario { get; init; }
    public string Competencia { get; init; }
    public TabelaInss InssVigente { get; init; }
    public TabelaIrrf IrrfVigente { get; init; }
    public TabelaSalarioFamilia SalFamVigente { get; init; }
    public decimal SalarioBaseVigente { get; init; }
    public int DependentesIrrf { get; init; }
    public int DependentesSf { get; init; }
    public decimal PensaoAlim { get; init; }
    public List<RubricaCalculada> Rubricas { get; } = new();
    public ResumoApontamentos Apontamentos { get; init; }   // de W2
    public List<BeneficioFuncionario> Beneficios { get; init; }
    public List<RubricaTenant> RubricasCustom { get; init; }
    public ParametrosCalculoFolha Param { get; init; }

    // Helpers CLT — ver implementação no design completo
    public decimal SalarioBaseProporcional() { ... }
    public decimal HoraExtra50() { ... }
    public decimal AdicionalNoturno() { ... }
    public decimal DescontoVT() { ... }
    public decimal AplicaTabelaInss() { ... }
    public decimal AplicaTabelaIrrf() { ... }
    public RubricaContexto AsRubricaContexto() { ... }  // para passar para evaluator
}
```

## Fixtures de validação (cobertura mínima)

```
fixtures/folha/
├── 01-clt-puro-3000.json                   sal 3000, sem nada
├── 02-clt-he-50-10h.json                   sal 3000, 10h HE 50%
├── 03-clt-he-100-2h.json                   sal 3000, 2h HE 100% (domingo)
├── 04-clt-noturno-8h.json                  sal 3000, 8h adicional 20%
├── 05-clt-peric-30pct.json                 sal 3000, periculosidade
├── 06-clt-insalub-grau-medio-20pct.json    sal 3000, insalub
├── 07-clt-2-dep-irrf.json                  sal 5000, 2 deps
├── 08-clt-vt-recebido.json                 sal 3000, VT 200
├── 09-clt-adiantamento-30pct.json          sal 3000, adto 30%
├── 10-clt-faltas-2-dias.json               sal 3000, 2 faltas
├── 11-clt-atestado-3-dias.json             sal 3000, atestado 3 dias (sem desc)
├── 12-clt-banco-horas-compensa-5h.json     sal 3000, compensa 5h
├── 13-clt-banco-horas-paga-10h.json        sal 3000, paga 10h banco
├── 14-clt-rubrica-custom-bonus.json        sal 3000, +bonus DSL
├── 15-clt-teto-inss.json                   sal 15000, teto INSS
├── 16-clt-isento-irrf.json                 sal 2000, isento IRRF
├── 17-clt-sal-fam-2-filhos.json
├── 18-mes-comecou-no-meio.json             admitido dia 15
├── 19-mes-terminou-no-meio.json            rescisão dia 15 (W9 expandirá)
├── 20-clt-pensao-alim-30pct.json
... (até 30)
```

Cada fixture: `input.json` (cadastro + apontamento) + `expected.json` (cada rubrica esperada).

## Performance

- 1 worker = ~100 funcionários/min (estimado).
- 4 workers paralelos → ~25 segundos para 100 funcionários, ~4min para 1000.
- DB queries por funcionário: ~10 (cadastro, salário, dependentes, benefícios, apontamentos, rubricas, tabelas com cache).
- Cache agressivo de tabelas legais (1h TTL).

## Tradeoffs

### Por que rubrica custom em DSL e não em código?

Cada tenant tem rubricas específicas (bônus, comissão regional, prêmio metas, descontos internos). Codificar em C# exigiria release a cada nova rubrica. DSL via W5 desacopla.

### Por que rodar via worker e não inline?

100 funcionários × 200ms = 20s. 1000 × 200ms = 200s. Worker paralelo + RabbitMQ é necessário para escalar.

### Por que não cachear holerite?

Cada cálculo depende de N entidades mutáveis (apontamento ajustado, salário corrigido). Cache invalidaria com frequência. Decisão: recalcular sob demanda em vez de cachear.

### Por que separar conferência de fechamento?

Status `Conferida` permite RH revisar antes do irreversível `Fechada`. Cada um exige permissão diferente.

## Test strategy

- **Unit**: 30 fixtures × engine → expected.
- **Unit**: cada helper do contexto (HoraExtra50, AplicaTabelaInss, etc.) com 5 cenários.
- **Integration**: abrir → calcular 100 funcs em worker → conferir → fechar → PDFs gerados.
- **Property-based**: total_proventos - total_descontos == liquido (invariante).
- **Smoke contra contador externo**: 5 holerites com cálculo manual de planilha — bate ao centavo.
