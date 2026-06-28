## ADDED Requirements

### Requirement: S-1200 gerado por holerite fechado

Para cada `HoleriteFuncionario` em `FolhaMensal.Fechada`, o sistema SHALL gerar 1 evento S-1200 com rubricas mapeadas, bases e incidências, enviado via pipeline W11.

#### Scenario: Folha 10 funcionários gera 10 S-1200

- **GIVEN** folha 2026-06 fechada com 10 holerites
- **WHEN** hook é disparado
- **THEN** sistema cria 10 EventoEsocial S-1200, um por funcionário
- **AND** cada um com perApur=2026-06, rubricas mapeadas via natureza eSocial

### Requirement: S-1210 gerado por pagamento confirmado

Quando `ContaPagar` vinculada a `LancamentoFinanceiroFolha` tipo `LiquidoFuncionario` é marcada Paga (W10 reverse sync), o sistema SHALL gerar S-1210 com data e valor.

#### Scenario: Pagamento de líquido dispara S-1210

- **GIVEN** ContaPagar do líquido do funcionário X (folha 2026-06) recém-paga
- **WHEN** evento `ContaPagarFoiPagaEvent` é publicado
- **THEN** hook cria `EventoEsocial { tipo: "S-1210", funcionarioId: X, perApur: 2026-06, vrLiq: <valor>, dtPgto: <data> }`

### Requirement: S-1299 fecha competência só com S-1200/S-1210 Aceitos

O sistema SHALL validar antes de enviar S-1299 que todos os S-1200 e S-1210 da competência estão em status Aceito. Endpoint retorna 409 se houver pendências.

#### Scenario: Fechar com pendências falha

- **GIVEN** competência 2026-06 tem 8 S-1200 Aceito + 2 S-1200 Rejeitado
- **WHEN** RH chama `POST /esocial/periodicos/2026-06/fechar`
- **THEN** sistema retorna 409 com `{ s1200Pendentes: 2, s1210Pendentes: 0 }`
- **AND** S-1299 não é gerado

#### Scenario: Fechar sem pendências sucesso

- **GIVEN** todos S-1200/S-1210 da competência Aceito
- **WHEN** RH fecha
- **THEN** sistema gera S-1299 e enfileira
- **AND** ao Aceito, competência fica fechada no eSocial

### Requirement: Retificação por NSR

Sistema SHALL prover endpoint `POST /esocial/eventos/{id}/retificar` que cria novo evento com `indRetif=2` referenciando o anterior via `nrRecibo`.

#### Scenario: Retificar S-1200 cria novo com indRetif=2

- **GIVEN** S-1200 Aceito com recibo R1
- **WHEN** RH retifica corrigindo campo
- **THEN** novo evento S-1200 criado com `indRetif=2, nrRecibo=R1`
- **AND** evento anterior marca Retificado

### Requirement: Reabertura de competência via S-3000

Após S-1299 Aceito, retificações exigem reabertura: novo S-3000 apontando para o S-1299. Sistema SHALL orquestrar o fluxo de reabertura.

#### Scenario: Reabertura sequencial

- **GIVEN** competência 2026-05 fechada (S-1299 Aceito)
- **WHEN** RH chama `POST /esocial/periodicos/2026-05/reabrir`
- **THEN** sistema gera S-3000 referenciando o S-1299
- **AND** após Aceito, competência fica reaberta
- **AND** RH pode retificar S-1200 ou S-1210 e depois fechar novamente
