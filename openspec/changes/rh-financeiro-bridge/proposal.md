## Why

W10. Folha calculada (W6/W7/W8/W9) gera **obrigações financeiras** que precisam virar lançamentos no módulo Financeiro do Atena (já existente). Esta onda liga RH a Financeiro:
- **Líquido por funcionário** → 1 ContaPagar por funcionário com data de pagamento + dados bancários.
- **Guia GPS (INSS)** → 1 ContaPagar agregada do INSS empregado + patronal + terceiros (RAT, SAT, salário-educação) com vencimento dia 20 mês seguinte.
- **DARF IRRF** → 1 ContaPagar agregada com vencimento conforme tabela.
- **GRF FGTS** → 1 ContaPagar agregada do FGTS empregador, vencimento dia 7 mês seguinte.
- **GRRF FGTS rescisão** → 1 ContaPagar individual quando há rescisão (vencimento 10 dias após desligamento).

Também:
- Conciliação reversa (pagamento marcado no Financeiro atualiza status do holerite).
- Rateio por centro de custo (folha já tem cargo+lotação).
- Relatório consolidado pagamentos × folha.

## What Changes

### Novas entidades

- `LancamentoFinanceiroFolha`
  - tenant_id, folha_id, tipo (`LiquidoFuncionario`, `GpsInss`, `DarfIrrf`, `GrfFgts`, `GrrfFgts`)
  - referencia_id (FK opcional para HoleriteFuncionario ou Rescisao)
  - conta_pagar_id (FK Financeiro)
  - valor, vencimento, descricao
  - status (`Pendente`, `Lançado`, `Pago`, `Cancelado`)

- `RateioCentroCustoFolha`
  - lancamento_id, centro_custo_id, percentual, valor_rateado

### Engine — `GeradorLancamentosFinanceirosFolha`

```
Trigger: FolhaMensal.Fechada
  ├── Para cada HoleriteFuncionario:
  │   └── Cria ContaPagar { valor: liquido, vencimento: folha.dataPgto,
  │                          beneficiario: funcionario, contaBancaria, descricao }
  │       ├── associa LancamentoFinanceiroFolha tipo LiquidoFuncionario
  │       └── distribui rateio por centro de custo do funcionário
  │
  ├── Agrega INSS:
  │   ├── total_inss_empregado = sum(holerites.rubrica '100-INSS-DESC')
  │   ├── inss_patronal = sum(holerites.base_inss) × 20% (regra geral; pode ter desoneração)
  │   ├── rat_sat = sum(holerites.base_inss) × empresa.rat_pct (1/2/3% conforme grau)
  │   ├── terceiros = sum(holerites.base_inss) × empresa.terceiros_pct (5.8% padrão)
  │   ├── total_gps = inss_empregado + patronal + rat + terceiros
  │   └── Cria 1 ContaPagar { valor: total_gps, vencimento: dia 20 mes+1, descrição "GPS competência ..." }
  │
  ├── Agrega IRRF:
  │   ├── total = sum(holerites.rubrica '110-IRRF-DESC')
  │   └── DARF cod. 0561 (rendimentos do trabalho) — vencimento conforme calendário
  │
  ├── Agrega FGTS:
  │   ├── total = sum(holerites.rubrica '900-FGTS-INFO')
  │   └── GRF/eSocial — vencimento dia 7 mes+1
  │
  └── Para cada Rescisao Concluida:
      ├── Cria ContaPagar tipo GRRF (multa 40% × saldo fgts)
      └── Vencimento: 10 dias após desligamento (rescisões CLT pós-2017)
```

### Endpoints

```
POST /api/v1/rh/folha/{folhaId}/gerar-lancamentos-financeiros
GET  /api/v1/rh/folha/{folhaId}/lancamentos-financeiros
GET  /api/v1/rh/folha/{folhaId}/conciliacao              status pago/pendente
POST /api/v1/rh/folha/{folhaId}/cancelar-lancamentos
GET  /api/v1/rh/relatorios/pagamentos-vs-folha?competencia=
```

### Reverse sync — pagamento atualiza holerite

```
Quando ContaPagar é marcada como Paga no Financeiro:
  Se vinculada a LancamentoFinanceiroFolha:
    Atualiza status do lançamento
    Notifica funcionário (se LiquidoFuncionario) — "Holerite pago em DD/MM"
```

Implementação: hook no `PagarContaPagarCommandHandler` (módulo Financeiro) que dispara evento `PagamentoFolhaRealizado`. Listener no RH atualiza.

### Rateio por centro de custo

```
Funcionário X tem CentroCustoId Y → 100% do líquido X é rateado em Y.

Empresa pode definir rateios mais complexos (não no MVP):
  Funcionário X → 70% CC-A + 30% CC-B (ações futuras).
```

## Capabilities

### New Capabilities
- `rh-financeiro-bridge` — Ponte automática folha → ContaPagar com agregações de guias INSS/IRRF/FGTS.

### Modified Capabilities
- `rh-folha` — Folha.Fechada dispara geração de lançamentos.
- `financeiro` (existente) — `ContaPagar` ganha campo `origem_folha_id` opcional + handler de pagamento que sincroniza.

## Out of Scope
- Geração do arquivo CNAB/SISPAG real (banco-específico) — pode ser change separado se cliente exigir.
- Emissão de boleto de FGTS pela Caixa (não há API pública aberta).
- Pagamento PIX automatizado.

## Risks

- **R1**: Lançamentos em duplicidade se folha for reaberta. Mitigação: cancelar lançamentos automaticamente em reabertura + recriar.
- **R2**: Datas de vencimento mudam (feriado, dia útil). Mitigação: helper `proximoDiaUtil(data, uf)` que ajusta.
- **R3**: Alíquota RAT/Terceiros varia por empresa. Mitigação: campos em `ConfiguracaoEmpresa` (já existe ou estender W1).
- **R4**: Pagamento manual sem vínculo cria divergência. Mitigação: relatório de divergência (pagamentos sem folha vinculada).

## Success Criteria

- Folha fechada de 100 funcionários gera 100 ContaPagar de líquido + 4 ContaPagar agregadas (GPS/DARF/GRF + eventualmente GRRF), tudo idempotente.
- Reabrir folha cancela lançamentos antigos.
- Conciliação reversa funciona (pagamento manual no Financeiro atualiza folha).
- Relatório "Pagamentos × Folha" mostra divergências.
- `openspec validate rh-financeiro-bridge --strict` válido.
