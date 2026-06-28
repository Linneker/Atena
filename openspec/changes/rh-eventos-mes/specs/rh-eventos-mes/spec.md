## ADDED Requirements

### Requirement: Controle de período aquisitivo de férias CLT

O sistema SHALL manter `Ferias` para cada funcionário, com período aquisitivo de 12 meses a partir da admissão (e ciclos), e SHALL calcular `dias_direito` conforme CLT art. 130 (5 cenários por faixa de faltas: 30, 24, 18, 12, 0 dias).

#### Scenario: Funcionário com 7 faltas perde 6 dias de férias

- **GIVEN** funcionário completou período aquisitivo com 7 faltas
- **WHEN** sistema atualiza `Ferias` ao fim do período
- **THEN** `dias_direito = 24` (faixa 6-14 faltas)

#### Scenario: Programar férias com venda

- **WHEN** RH programa férias `{ dataInicio: "2026-07-01", diasGozar: 20, diasVender: 10, adiantar13o: true }`
- **THEN** sistema cria FolhaMensal tipo Ferias com rubricas:
  - F01-SAL-FERIAS (proporcional a 30 dias)
  - F02-1-3-CONST (1/3 constitucional)
  - F03-ABONO-PEC (10 dias × diário × 1.333)
  - F04-ADTO-13 (1/12 × meses trabalhados / 2)
  - F100-INSS-FER, F110-IRRF-FER (descontos)

### Requirement: 13º salário em 2 parcelas com cálculo CLT

O sistema SHALL calcular 13º proporcional aos meses trabalhados no ano (≥15 dias no mês conta), com 1ª parcela = 50% sem desconto (pago até 30/nov) e 2ª parcela = total - 1ª - INSS - IRRF (pago até 20/dez).

#### Scenario: 1ª parcela 13º

- **GIVEN** funcionário sal R$3.000, admitido em 2026-03-10 (10 meses até 2026-12, mas precisa ≥15 dias no mês de admissão: março tem 22 dias trabalhados → conta)
- **WHEN** sistema processa 1ª parcela em novembro
- **THEN** valor = R$3.000 / 12 × 10 × 0.5 = R$1.250,00
- **AND** sem desconto

#### Scenario: 2ª parcela 13º com INSS e IRRF

- **GIVEN** mesmo funcionário em dezembro
- **WHEN** sistema processa 2ª parcela
- **THEN** total = R$3.000 / 12 × 10 = R$2.500,00
- **AND** desconta INSS sobre R$2.500
- **AND** desconta IRRF sobre (R$2.500 - INSS - 189.59 × deps)
- **AND** 2ª parcela = total - R$1.250 - INSS - IRRF

### Requirement: Afastamento com split empresa/INSS após 15 dias

O sistema SHALL controlar afastamentos com `data_inicio` e `data_fim` (NULL=aberto), e SHALL emitir alerta automático quando afastamento por atestado/doença ultrapassar 15 dias (INSS assume).

#### Scenario: Atestado de 20 dias

- **GIVEN** afastamento tipo Atestado iniciado em 2026-06-01
- **WHEN** chega 2026-06-16
- **THEN** sistema cria alerta "Funcionário X precisa abrir benefício INSS"
- **AND** folha do mês paga apenas 15 dias normais; dias 16-20 com rubrica informativa "INSS"

### Requirement: Job automático de pendências

O sistema SHALL ter jobs noturnos/mensais que:
1. Criam `Ferias.Pendente` para funcionários com período aquisitivo recém-completo;
2. Alertam férias vencendo (após 12 meses do fim do período sem gozo, empresa paga em dobro);
3. Processam 13º proporcional automaticamente em 15/nov e 15/dez (configurável).

#### Scenario: Job cria férias pendentes

- **GIVEN** funcionário X completou período aquisitivo em 2026-05-31 sem férias pendentes
- **WHEN** job mensal roda em 2026-06-01
- **THEN** cria `Ferias { status: Pendente, periodoInicio: 2025-06-01, periodoFim: 2026-05-31, diasDireito: 30 }`
- **AND** notifica RH

### Requirement: Aviso de Férias PDF

O sistema SHALL gerar PDF "Aviso de Férias" (CLT art. 135) quando férias são programadas, contendo: período aquisitivo, dias a gozar, datas de início/fim/pagamento, espaço para assinaturas.

#### Scenario: PDF gerado ao programar

- **WHEN** RH programa férias
- **THEN** sistema gera Aviso PDF e armazena em S3
- **AND** retorna URL na resposta da programação
