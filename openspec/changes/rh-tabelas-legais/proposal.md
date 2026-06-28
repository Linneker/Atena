## Why

W5. Antes de calcular folha (W6), o sistema precisa de **tabelas tributárias e auxiliares oficiais**, versionadas por competência e atualizáveis sem release. Esta onda entrega: tabelas INSS, IRRF, FGTS, salário-mínimo, salário-família, vale-transporte, calendário de feriados nacionais/regionais, naturezas de rubrica eSocial, e **rubricas customizadas por tenant** (Q2).

## What Changes

### Tabelas legais nacionais (não tenant-scoped)

- `tabela_inss` — faixas com alíquotas por competência (após Reforma Previdência 2019: até 4 faixas).
  - Schema: `id, competencia_inicio, competencia_fim (NULL=vigente), faixa_inicio, faixa_fim, aliquota_pct, parcela_deduzir`.
- `tabela_irrf` — faixas com alíquotas e parcela a deduzir, dependentes e simplificado.
- `tabela_fgts` — alíquota normal (8%), alíquota multa rescisão (40%), alíquota multa CESEF (10% suspenso, manter campo).
- `salario_minimo_nacional` — valor + competência início + fim.
- `salario_minimo_regional` — opcional, por UF.
- `tabela_salario_familia` — limite de remuneração + valor da cota.
- `tabela_vale_transporte` — desconto máx 6% do salário base (regra fixa, persistida para auditoria).
- `tabela_feriados_nacionais` — data, descrição, fixo/móvel.
- `tabela_feriados_estaduais` — uf, data, descrição.
- `tabela_feriados_municipais` — codigo_ibge_municipio, data, descrição.
- `naturezas_rubrica_esocial` — código S-1010 oficial (proventos: 1xxx; descontos: 9xxx) — semeado por migration.

### Rubricas tenant (Q2)

- `rubricas_tenant`
  - tenant_id, codigo (PK por tenant), descricao
  - tipo (`Provento`, `Desconto`, `Informativa`)
  - natureza_esocial (FK opcional → naturezas_rubrica_esocial)
  - formula_dsl (TEXT, expressão da fórmula — DSL minimalista)
  - incidencias (bits: incideINSS, incideIRRF, incideFGTS, incideFerias, incide13o, incideDSR)
  - dependencias_outras_rubricas (lista — para ordem de cálculo)
  - vigencia_inicio, vigencia_fim
  - ativa BOOLEAN

- `rubricas_catalogo_nacional` (somente leitura, semeado) — modelos de rubricas comuns (salário-base, HE 50%, HE 100%, DSR, adicional noturno, peric, insalub, VT, VR, INSS desc, IRRF desc, etc.) que o tenant pode clonar e ajustar.

### DSL de fórmula

Mini-linguagem de expressão:
- Variáveis: `salarioBase`, `horasNormais`, `horasExtras50`, `horasExtras100`, `valorVT`, `qtdDependentes`, etc.
- Operadores: `+ - * / %` e funções `min(a,b)`, `max(a,b)`, `if(cond, then, else)`.
- Constantes: `tabelaInssFaixas`, `salarioMinimoVigente`.
- Exemplos:
  - HE 50%: `(salarioBase / 220) * horasExtras50 * 1.5`
  - INSS: `aplicaTabelaInss(remuneracaoBruta)` (função built-in)
  - VT desconto: `min(salarioBase * 0.06, valorVTBenefico)`

DSL executada via **interpretador safe** (não Roslyn dinâmico) — `RubricaExpressionEvaluator`. Decisão: começar com gramática enxuta + ANTLR ou Sprache (lib parser). Avaliar `NCalc` (lib pronta, mas precisa sandbox).

### Endpoints admin — upload de tabelas (Q6)

```
POST /api/v1/admin/rh/tabelas/{tipo}/upload
  multipart: arquivo (JSON ou CSV) + competencia + override?
  tipos: inss, irrf, fgts, salario-minimo, salario-familia, feriados-nacionais, feriados-estaduais, feriados-municipais, naturezas-esocial

Permissão: admin:upload-tabelas-legais (Root + nova role RhAdmin)
```

Vigência: `competencia` no upload define `competencia_inicio`; o endpoint **fecha automaticamente** o `competencia_fim` da vigência anterior (= competência nova - 1 mês). Override permite reescrever vigências sobrepostas.

### Endpoints públicos (autenticados) — consulta

```
GET /api/v1/rh/tabelas/inss?em=2026-06
GET /api/v1/rh/tabelas/irrf?em=2026-06
GET /api/v1/rh/tabelas/salario-minimo?em=2026-06
GET /api/v1/rh/tabelas/feriados?em=2026-06&uf=SP&municipio=3550308
GET /api/v1/rh/tabelas/naturezas-esocial
```

### Endpoints tenant — CRUD rubricas

```
GET    /api/v1/rh/rubricas
POST   /api/v1/rh/rubricas
GET    /api/v1/rh/rubricas/{codigo}
PUT    /api/v1/rh/rubricas/{codigo}
DELETE /api/v1/rh/rubricas/{codigo}
POST   /api/v1/rh/rubricas/clonar-do-catalogo/{codigoNacional}
POST   /api/v1/rh/rubricas/{codigo}/testar { contextoSimulado: {...} }
```

### Cache

Tabelas legais variam pouco — cache distribuído (Redis, já presente) com TTL = 1h e invalidação por upload.

### Permissions

- `Recursos.AdminTabelasLegais` + `Acoes.Upload`, `Acoes.Listar`
- `Recursos.RhRubrica` + CRUD padrão + `Acoes.Testar`

### Tabelas iniciais (vigências 2026)

Seeds inline na migration com dados oficiais 2026:
- INSS 2026: 4 faixas (7.5%, 9%, 12%, 14%) com tetos.
- IRRF 2026: tabela mensal vigente.
- FGTS: 8% padrão.
- SM nacional 2026: R$1.518,00 (valor exemplar; ajustar quando confirmado).
- Feriados nacionais 2026 + bissextos fixos.

## Capabilities

### New Capabilities
- `rh-tabelas-legais` — Tabelas tributárias e auxiliares brasileiras versionadas; upload admin; rubricas por tenant com DSL.

### Modified Capabilities
- `seed-tenant-administrativo` — semeia catálogo de rubricas modelo do tenant + 5-10 rubricas básicas vigentes.

## Out of Scope
- CCT-específicas (W7).
- Cálculo de folha (W6).
- IRPF anual de pessoa física (não é IRRF).
- Rubricas com dependências circulares (validador rejeita).

## Risks

- **R1**: Tabelas mudam por MP/Decreto fora do release. Q6 endossado.
- **R2**: DSL muito permissiva vira buraco de segurança (eval arbitrário). Mitigação: gramática estritamente limitada + sandbox.
- **R3**: Rubricas circulares (A depende de B que depende de A). Mitigação: validador de dependencies no save + topological sort.
- **R4**: SM regional muda por UF independentemente. Mitigação: tabela separada com override.

## Success Criteria

- Tabelas INSS/IRRF/SM 2026 carregadas via seed e consultáveis via API.
- Upload de nova competência via admin funciona idempotente (re-upload mesmo período substitui).
- Tenant cria rubrica customizada "Bônus mensal" com DSL `if(metaAtingida, salarioBase * 0.1, 0)` e testa via endpoint `/testar`.
- Cache invalida na atualização.
- DSL passa em 50 fixtures (incluindo casos malicioso bloqueados).
- `openspec validate rh-tabelas-legais --strict` válido.
