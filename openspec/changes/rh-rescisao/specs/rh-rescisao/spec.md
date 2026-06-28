## ADDED Requirements

### Requirement: Cálculo de rescisão por tipo CLT

O sistema SHALL calcular rescisão conforme o tipo escolhido (8 tipos), aplicando os direitos previstos em CLT (aviso prévio, multa FGTS, férias proporcionais, 13º proporcional, seguro-desemprego) via tabela `RegrasRescisao.Direitos`.

#### Scenario: Sem Justa Causa — empresa

- **GIVEN** funcionário sal R$4.000, admitido 2023-01-15, rescisão tipo `SemJustaCausaEmpresa` em 2026-06-30
- **WHEN** engine calcula
- **THEN** holerite contém:
  - R01-SAL-SALDO (30/30 dias)
  - R02-AVISO-IND (30d + 3d × 3 anos = 39d indenizado)
  - R03-FER-VENC + 1/3
  - R04-FER-PROP (6 meses) + 1/3
  - R05-13-PROP (6 meses)
  - R06-MULTA-FGTS (40% sobre saldo informado)
  - R100/110 descontos

#### Scenario: Justa Causa — perde quase tudo

- **GIVEN** rescisão tipo `JustaCausaEmpresa`
- **WHEN** engine calcula
- **THEN** apenas R01 (saldo salário) e R03 (férias VENCIDAS, não proporcionais)
- **AND** sem aviso, sem multa FGTS, sem 13º prop, sem férias prop, sem seguro-desemprego

#### Scenario: Acordo consensual — verbas pela metade

- **WHEN** rescisão tipo `AcordoConsensual`
- **THEN** aviso prévio indenizado = 50% do calculado
- **AND** multa FGTS = 20% (em vez de 40%)
- **AND** sem seguro-desemprego

### Requirement: TRCT PDF oficial vigente

O sistema SHALL gerar TRCT PDF conforme NR-127 (formulário oficial MTE vigente), com identificação completa, tabela de rubricas, totais, e espaço para assinaturas (empregado, empresa, homologador).

#### Scenario: TRCT em rascunho marca d'água

- **GIVEN** rescisão em status Calculada
- **WHEN** `GET /rescisoes/{id}/trct.pdf`
- **THEN** PDF tem marca d'água "RASCUNHO"

#### Scenario: TRCT homologado limpo

- **GIVEN** rescisão Homologada
- **WHEN** `GET /rescisoes/{id}/trct.pdf`
- **THEN** PDF sem marca d'água
- **AND** contém dados de homologação (data, local, homologador)

### Requirement: Workflow Programada → Concluida com efeitos colaterais

O sistema SHALL gerenciar rescisão como máquina de estados Programada → Calculada → Homologada → Concluida. Ao concluir, SHALL:
1. marcar `Funcionario.status=Desligado` e `dataDemissao`;
2. marcar `Usuario.status=Desativado`;
3. criar pendência para W10 (Financeiro);
4. criar pendência para W13 (eSocial S-2299).

#### Scenario: Concluir rescisão desliga funcionário

- **GIVEN** rescisão Homologada
- **WHEN** RH chama `POST /rescisoes/{id}/concluir`
- **THEN** sistema transita para Concluida
- **AND** funcionário fica Desligado + Usuario Desativado
- **AND** próxima tentativa de login do Usuario retorna 403
- **AND** pendência aparece em fila W10 e W13

### Requirement: Saldo FGTS informado manualmente

O sistema SHALL aceitar `saldoFgtsConhecido` no momento da criação da rescisão (não tenta consultar Caixa). Multa de 40% calcula sobre esse valor.

#### Scenario: RH informa saldo FGTS na criação

- **WHEN** RH cria rescisão `{ ..., saldoFgtsConhecido: 12500.00 }`
- **AND** tipo `SemJustaCausaEmpresa`
- **THEN** R06-MULTA-FGTS = R$5.000,00 (40% × 12.500)
