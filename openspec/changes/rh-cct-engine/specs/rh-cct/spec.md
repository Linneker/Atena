## ADDED Requirements

### Requirement: Modelagem estruturada de Convenção Coletiva

O sistema SHALL modelar Convenções e Acordos Coletivos como entidade `Convencao` por tenant, com vigência, sindicato, categoria, e múltiplas `RegraConvencao` polimórficas (tipo + parâmetros JSON). SHALL suportar 15 tipos de regra (Piso Salarial, HE %, Anuênio, Periculosidade, etc.) mais escape hatch `RegraCustomDsl`.

#### Scenario: Criar CCT metalúrgicos SP

- **WHEN** RH cria convenção `{ codigo: "METAL-SP-2026", categoriaProfissional: "Metalúrgicos", sindicatoCnpj: "...", vigenciaInicio: "2026-05-01" }`
- **AND** adiciona regra `{ tipo: "AdicionalHeDiurnoPct", parametros: { pct: 60, comDsr: true } }`
- **AND** adiciona regra `{ tipo: "AnueniePct", parametros: { pctPorAno: 1, tetoAnos: 25 } }`
- **THEN** sistema persiste convenção + 2 regras

### Requirement: Resolução de convenção aplicável (override → adesão → nenhuma)

O sistema SHALL resolver a Convenção aplicável para um funcionário em uma competência seguindo a precedência:
1. `OverrideConvencaoFuncionario` vigente (se existir);
2. `AdesaoConvencao` vigente da empresa do funcionário (pode ser múltiplas);
3. Sem convenção → engine usa fallback CLT base.

#### Scenario: Funcionário com override

- **GIVEN** funcionário X vinculado à empresa Y; empresa Y aderiu à CCT-A
- **AND** funcionário X tem override individual para CCT-B
- **WHEN** resolvedor é chamado para competência atual
- **THEN** retorna CCT-B (override prevalece)

#### Scenario: Sem convenção aplicável

- **WHEN** funcionário Z sem override e sua empresa sem adesão
- **THEN** resolvedor retorna `null`
- **AND** engine de folha usa percentuais CLT padrão (HE 50%, noturno 20%, etc.)

### Requirement: Aplicação automática de regra no engine de folha

Engine de folha (W6) SHALL invocar handler de cada `RegraConvencao` aplicável, na ordem definida (`ordem ASC`), modificando o contexto da folha (PctHeDiurno, PisoSalarial, etc.) ou adicionando rubricas (Anuênio, Auxílios).

#### Scenario: CCT com HE 60% sobrepõe default 50%

- **GIVEN** funcionário fez 10h HE diurna; CCT vigente tem `AdicionalHeDiurnoPct { pct: 60 }`
- **WHEN** engine calcula
- **THEN** rubrica 020-HE-50 vira `020-HE-60: salBase/220 * 10 * 1.6`

#### Scenario: Anuênio 1% por ano com teto 25

- **GIVEN** funcionário admitido em 2010-03-01 (16 anos em 2026-06); CCT tem `AnueniePct { pctPorAno: 1, tetoAnos: 25 }`
- **WHEN** engine calcula em 2026-06
- **THEN** adiciona rubrica `ANU-CCT: salBase * 0.16` (16% do sal base)

### Requirement: Simulação de impacto antes de ativar

O sistema SHALL prover `POST /rh/convencoes/{id}/simular-impacto` que calcula holerite COM e SEM a convenção para um conjunto de funcionários e retorna diff legível (rubricas novas, valores alterados, delta de líquido).

#### Scenario: Simular impacto em 5 funcionários

- **WHEN** RH simula CCT nova em 5 funcionários para competência 2026-06
- **THEN** sistema executa engine 2x por funcionário (sem + com CCT)
- **AND** retorna `{ funcionarios: [{ funcId, baseLiquido, ccLiquido, delta, rubricasNovas: ["ANU-CCT"], rubricasAlteradas: ["020-HE-50→60"] }] }`

### Requirement: Marcação de folhas afetadas para recálculo

Quando Convenção/Regra/Adesão é alterada e a mudança afeta competências de folhas em status `Calculada` ou `Conferida` (não `Fechada`), o sistema SHALL marcar essas folhas com `precisa_recalcular=true` e notificar o RH.

#### Scenario: Editar regra de CCT vigente

- **GIVEN** existem 2 folhas Calculadas (2026-05 e 2026-06) na empresa com adesão à CCT
- **WHEN** RH muda `AdicionalHeDiurnoPct` de 50 para 60
- **THEN** ambas as folhas recebem `precisa_recalcular=true`
- **AND** notificação no bell aparece para RH

### Requirement: Escape hatch via RegraCustomDsl

Para regras não-padronizáveis, o sistema SHALL suportar tipo `RegraCustomDsl` que carrega expressão na DSL definida em W5, executada via `RubricaExpressionEvaluator` sandbox.

#### Scenario: CCT com gatilho exótico

- **WHEN** RH cria regra `{ tipo: "RegraCustomDsl", parametros: { dsl: "if(qtdAnosCasaCheios >= 10, salarioBase * 0.05, 0)" } }`
- **THEN** engine adiciona rubrica `CCT-CUSTOM` com valor resultante da avaliação no contexto
