## ADDED Requirements

### Requirement: Cálculo de folha mensal CLT por funcionário

O sistema SHALL calcular a folha mensal de cada funcionário CLT a partir de: salário vigente, apontamentos do mês (W2), dependentes, benefícios, rubricas custom do tenant (W5), e tabelas legais vigentes (W5). SHALL produzir `HoleriteFuncionario` com rubricas, totais, bases, e valor líquido.

#### Scenario: Funcionário CLT padrão 3000

- **GIVEN** funcionário com salário R$3000, 0 dependentes, jornada 44h cumprida sem HE
- **WHEN** engine calcula folha competência 2026-06
- **THEN** holerite contém:
  - 001-SAL-BASE: R$3.000,00 (provento)
  - 100-INSS-DESC: -R$248,77 (desconto, aplicando tabela 2026)
  - 110-IRRF-DESC: R$0 (isento, base 2.751,23)
  - 900-FGTS-INFO: R$240,00 (informativo)
- **AND** total proventos: R$3.000,00
- **AND** total descontos: R$248,77
- **AND** líquido: R$2.751,23

#### Scenario: Funcionário com horas extras

- **GIVEN** funcionário sal R$3.300, 10h HE 50%
- **WHEN** engine calcula
- **THEN** holerite contém rubrica `020-HE-50: R$225,00` (= 3300/220 * 10 * 1.5)
- **AND** HE entra na base INSS e IRRF

### Requirement: Workflow Aberta → Calculada → Conferida → Fechada

`FolhaMensal` SHALL passar pelos estados Aberta → EmCalculo → Calculada → Conferida → Fechada. SHALL haver transição administrativa Fechada → Reaberta. Edição direta de holerites SHALL ser bloqueada após `Fechada`, exigindo reabertura explícita.

#### Scenario: Fechar folha trava edição

- **GIVEN** folha em status Conferida
- **WHEN** RH chama `POST /folha/{id}/fechar`
- **THEN** status vira Fechada
- **AND** tentativa de recalcular holerite individual retorna 409
- **AND** dispara worker de PDFs em massa
- **AND** dispara pendência para W10 (Financeiro) e W14 (eSocial)

#### Scenario: Admin reabre folha fechada

- **GIVEN** folha Fechada de 2026-05
- **WHEN** admin `POST /folha/{id}/reabrir { motivo: "Erro identificado em VT" }`
- **THEN** status → Reaberta, audit log com motivo
- **AND** pode chamar /calcular para recálculo

### Requirement: Cálculo de 100 funcionários em ≤ 3 minutos

O motor SHALL ser capaz de processar folha de 100 funcionários ativos em até 3 minutos via worker paralelo (RabbitMQ), com tratamento isolado de erros.

#### Scenario: Erro em 1 funcionário não derruba o lote

- **GIVEN** folha com 100 funcionários, sendo que funcionário X tem rubrica custom com erro
- **WHEN** engine processa o lote
- **THEN** 99 holerites são gerados normalmente
- **AND** funcionário X tem entrada em `erros_calculo_folha` com mensagem
- **AND** status da folha vira Calculada (com aviso de N divergências)

### Requirement: Holerite PDF determinístico

O sistema SHALL gerar holerite PDF via QuestPDF de forma determinística: dadas mesmas entradas (cadastro, rubricas calculadas, tabelas), SHALL produzir mesmo arquivo PDF byte-identico.

#### Scenario: Regerar holerite produz mesmo arquivo

- **WHEN** sistema gera holerite do funcionário X em 2026-06 às 10:00
- **AND** gera novamente o mesmo holerite às 15:00
- **THEN** PDF é byte-identico (mesmo hash SHA-256)

### Requirement: Bateria de fixtures CLT brasileiras

O sistema SHALL ter ao menos 30 fixtures de cenários CLT (sal puro, HE, noturno, peric, insalub, dependentes, VT, banco horas, etc.) com inputs e expected outputs. SHALL haver teste parametrizado que roda todas as fixtures e compara com expected ao centavo.

#### Scenario: Bateria roda em CI

- **WHEN** CI executa `dotnet test --filter "Acao=CalcularFolha"`
- **THEN** 30 fixtures rodam e todas passam (valores conferidos ao centavo)
