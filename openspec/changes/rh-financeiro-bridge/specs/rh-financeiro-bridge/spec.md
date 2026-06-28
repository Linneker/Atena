## ADDED Requirements

### Requirement: Geração automática de ContaPagar ao fechar folha

Ao transitar `FolhaMensal.Fechada`, o sistema SHALL gerar idempotentemente:
1. N `ContaPagar` (uma por funcionário, valor=líquido, vencimento=dia de pagamento da empresa);
2. 1 `ContaPagar` agregada GPS (INSS empregado + patronal + RAT × FAP + terceiros);
3. 1 `ContaPagar` agregada DARF IRRF;
4. 1 `ContaPagar` agregada GRF FGTS;
5. Para cada rescisão Concluida nessa folha, 1 `ContaPagar` GRRF (multa 40% FGTS).

Todas vinculadas via `LancamentoFinanceiroFolha`.

#### Scenario: Folha 10 funcionários gera 14 ContasPagar

- **GIVEN** folha 2026-06 com 10 funcionários CLT, sem rescisões
- **WHEN** folha é fechada
- **THEN** sistema cria 10 ContaPagar de líquido + 1 GPS + 1 DARF + 1 GRF = 13 contas
- **AND** todas vinculadas a `LancamentoFinanceiroFolha` com `folha_id` correto

#### Scenario: Reabrir folha cancela lançamentos

- **GIVEN** folha 2026-06 fechada com 13 lançamentos
- **WHEN** admin reabre a folha
- **THEN** os 13 ContaPagar associados são cancelados (status=Cancelado)
- **AND** ao fechar novamente, novos lançamentos são gerados

### Requirement: Vencimentos brasileiros respeitam feriados

`VencimentosFolha` SHALL aplicar próximo dia útil quando vencimento cai em feriado/fim de semana, usando tabela de feriados de W5.

#### Scenario: GPS competência cai no domingo

- **GIVEN** competência 2026-06; dia 20/07/2026 é domingo
- **WHEN** sistema calcula vencimento GPS
- **THEN** retorna 21/07/2026 (segunda-feira)

#### Scenario: GRF cai em feriado

- **GIVEN** competência 2026-08; dia 07/09 é Independência (feriado nacional)
- **WHEN** sistema calcula vencimento GRF
- **THEN** retorna 08/09 (segunda)

### Requirement: Reverse sync — pagamento atualiza folha

Quando `ContaPagar` com `origem_folha_id` é marcada como Paga no módulo Financeiro, o sistema SHALL atualizar o `LancamentoFinanceiroFolha` correspondente para status `Pago` e notificar o funcionário (no caso de LiquidoFuncionario).

#### Scenario: Pagamento de líquido notifica funcionário

- **GIVEN** folha 2026-06 fechada; ContaPagar do líquido do funcionário X criada
- **WHEN** financeiro marca essa ContaPagar como Paga
- **THEN** `LancamentoFinanceiroFolha` correspondente recebe `status=Pago`
- **AND** funcionário X recebe notificação "Holerite 2026-06 pago em DD/MM"

### Requirement: Cálculo INSS empregador com RAT × FAP × Terceiros

O agregador GPS SHALL usar configuração tributária da empresa (rat_pct, fap_pct, terceiros_pct) para calcular o total a recolher.

#### Scenario: Cálculo GPS com RAT 2%, FAP 1.0, Terceiros 5.8%

- **GIVEN** baseInss agregado da folha = R$100.000
- **AND** INSS empregado agregado = R$11.000
- **AND** empresa { ratPct: 2.0, fapPct: 1.0, terceirosPct: 5.8 }
- **WHEN** agregador calcula
- **THEN** patronal = R$20.000 (20%)
- **AND** RAT = R$2.000 (2% × 1.0)
- **AND** terceiros = R$5.800
- **AND** total GPS = R$38.800

### Requirement: Idempotência da geração

A geração SHALL ser idempotente: chamar `GerarAsync(folhaId)` duas vezes sem reabertura no meio SHALL não duplicar lançamentos.

#### Scenario: Re-chamada sem reabertura é noop

- **GIVEN** folha fechada já tem lançamentos gerados
- **WHEN** sistema invoca `GerarAsync` novamente
- **THEN** nenhum novo lançamento é criado
- **AND** retorna sem erro
