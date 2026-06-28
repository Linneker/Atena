## ADDED Requirements

### Requirement: Tabelas legais versionadas por competência

O sistema SHALL manter tabelas INSS, IRRF, FGTS, salário-mínimo (nacional e regional), salário-família, vale-transporte, naturezas eSocial, e feriados (nacional/estadual/municipal), todas versionadas por `competencia_inicio` e `competencia_fim` (NULL=vigente). Consulta SHALL retornar a vigência correta para qualquer competência informada.

#### Scenario: Consulta tabela INSS de competência passada

- **GIVEN** tabela INSS tem vigências [2025-01..2025-12, 2026-01..NULL]
- **WHEN** consulta `GET /api/v1/rh/tabelas/inss?em=2025-08`
- **THEN** retorna a vigência 2025

#### Scenario: Vigência fechada automaticamente em novo upload

- **GIVEN** vigência atual INSS é 2026-01..NULL
- **WHEN** admin faz upload de INSS competência 2026-07
- **THEN** vigência anterior recebe `competencia_fim=2026-06`
- **AND** nova vigência é 2026-07..NULL
- **AND** consulta para 2026-05 ainda retorna a antiga; para 2026-07 retorna a nova

### Requirement: Upload admin de tabelas tributárias

O sistema SHALL prover endpoint `POST /api/v1/admin/rh/tabelas/{tipo}/upload` que aceita JSON ou CSV, valida formato e consistência (faixas não sobrepostas, somatórios, dependências), e persiste atomicamente fechando vigência anterior. Permissão `admin:upload-tabelas-legais` exclusiva de Root + role `RhAdmin`.

#### Scenario: Upload válido de IRRF

- **WHEN** admin envia arquivo JSON IRRF 2026-07 com 5 faixas válidas
- **THEN** sistema valida, persiste 5 linhas, fecha vigência anterior, invalida cache, retorna `{ totalInseridas: 5, vigenciaAnteriorFechada: "2026-01..2026-06" }`

#### Scenario: Upload com faixas sobrepostas é rejeitado

- **WHEN** admin envia JSON com faixa1 [0..2000] e faixa2 [1500..3000]
- **THEN** sistema retorna 400 com `Faixas sobrepostas: 1500 ∈ [0..2000] e [1500..3000]`
- **AND** nada é persistido

#### Scenario: Acesso negado para não-admin

- **WHEN** usuário sem `admin:upload-tabelas-legais` chama o endpoint
- **THEN** retorna 403

### Requirement: Rubricas customizáveis por tenant via DSL

O sistema SHALL permitir cada tenant criar suas próprias rubricas em `rubricas_tenant`, com fórmula expressa em DSL minimalista (operadores aritméticos, condicional, whitelist de funções built-in). Tenant SHALL poder clonar rubricas do `rubricas_catalogo_nacional` como ponto de partida.

#### Scenario: Tenant cria rubrica "Bônus mensal"

- **WHEN** tenant `POST /api/v1/rh/rubricas { codigo: "BONUS", descricao: "Bônus mensal", tipo: "Provento", formulaDsl: "if(metaAtingida, salarioBase * 0.1, 0)", incideInss: true, incideIrrf: true, incideFgts: true }`
- **THEN** sistema valida DSL (sintaxe + whitelist funções)
- **AND** persiste rubrica ativa

#### Scenario: DSL com função não whitelistada é rejeitada

- **WHEN** tenant envia formulaDsl `Process.Start("calc.exe")`
- **THEN** validador rejeita com `Função 'Process.Start' não permitida`

#### Scenario: Tenant testa rubrica com contexto simulado

- **WHEN** `POST /api/v1/rh/rubricas/BONUS/testar { contexto: { salarioBase: 3000, metaAtingida: true } }`
- **THEN** retorna `{ resultado: 300.00, tempoMs: 12 }`

### Requirement: Validação de dependências entre rubricas

O sistema SHALL detectar ciclos em dependências entre rubricas (A→B→A) e rejeitar a criação/edição. SHALL produzir ordem topológica para o engine de folha (W6) calcular rubricas na ordem correta.

#### Scenario: Ciclo é detectado e rejeitado

- **GIVEN** rubrica `A` depende de `B`
- **WHEN** tenant cria rubrica `B` com formula que depende de `A`
- **THEN** sistema rejeita com `Ciclo detectado: A → B → A`

### Requirement: Cache distribuído com invalidação por upload

O sistema SHALL cachear consultas a tabelas legais via Redis com TTL=1h, e SHALL invalidar caches do tipo afetado imediatamente após upload bem-sucedido (via evento `TabelasLegaisAtualizadas`).

#### Scenario: Cache invalida após upload INSS

- **GIVEN** consulta `GET /rh/tabelas/inss?em=2026-07` populou cache
- **WHEN** admin faz upload INSS 2026-07
- **THEN** chave `tabela:inss:*` é removida do Redis
- **AND** próxima consulta refaz query do DB e popula cache com novo valor

### Requirement: Seeds 2026 inline na migration

Migrations da onda SHALL trazer inline as tabelas oficiais com vigência iniciando em 2026-01 (INSS, IRRF, salário-mínimo, feriados nacionais 2026, naturezas S-1010 catálogo oficial).

#### Scenario: Tenant novo já tem tabelas vigentes

- **GIVEN** banco recém-migrado sem nenhum upload admin
- **WHEN** consulta `GET /api/v1/rh/tabelas/inss?em=2026-06`
- **THEN** retorna 4 faixas INSS 2026 (semeadas inline)
- **AND** consulta `GET /api/v1/rh/tabelas/salario-minimo?em=2026-06` retorna o SM 2026
- **AND** consulta `GET /api/v1/rh/tabelas/feriados?em=2026-06` retorna feriados nacionais de 2026
