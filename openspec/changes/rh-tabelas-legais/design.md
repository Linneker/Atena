# Design — rh-tabelas-legais

## Modelo de tabelas legais — padrão "vigência por competência"

```
┌─────────────────────────────────────────────────────────────┐
│  ESTRATÉGIA GENÉRICA: TabelaXxx                             │
├─────────────────────────────────────────────────────────────┤
│ competencia_inicio  competencia_fim  payload               │
│ 2024-01             2024-12          { ... versão antiga } │
│ 2025-01             2025-12          { ... versão 2025 }   │
│ 2026-01             NULL             { ... vigente }       │
└─────────────────────────────────────────────────────────────┘
```

Consulta por competência:
```sql
SELECT * FROM tabela_inss
WHERE competencia_inicio <= '2026-06'
  AND (competencia_fim IS NULL OR competencia_fim >= '2026-06')
```

## Esquema das principais tabelas

### `tabela_inss`
```sql
CREATE TABLE tabela_inss (
  id CHAR(36) PRIMARY KEY,
  competencia_inicio CHAR(7),       -- "YYYY-MM"
  competencia_fim CHAR(7),          -- NULL = vigente
  ordem_faixa TINYINT,
  faixa_inicio DECIMAL(12,2),
  faixa_fim DECIMAL(12,2),
  aliquota_pct DECIMAL(5,2),
  parcela_deduzir DECIMAL(10,2),    -- 0 se usar tabela escalonada pura
  seed_origem ENUM('migration','upload-admin'),
  importado_em DATETIME(6),
  importado_por CHAR(36),
  UNIQUE KEY uk (competencia_inicio, ordem_faixa)
);
```

### `tabela_irrf` (mesmo padrão)

### `salario_minimo_nacional`
```sql
id, competencia_inicio, competencia_fim, valor DECIMAL(10,2),
seed_origem, importado_em
```

### `rubricas_tenant`
```sql
CREATE TABLE rubricas_tenant (
  id, tenant_id,
  codigo VARCHAR(20),
  descricao VARCHAR(200),
  tipo ENUM('Provento','Desconto','Informativa'),
  natureza_esocial_codigo VARCHAR(10),
  formula_dsl TEXT,
  incide_inss BOOLEAN,
  incide_irrf BOOLEAN,
  incide_fgts BOOLEAN,
  incide_ferias BOOLEAN,
  incide_13o BOOLEAN,
  incide_dsr BOOLEAN,
  dependencias_json JSON,            -- ["sal-base","hr-extra-50"]
  vigencia_inicio DATE,
  vigencia_fim DATE,
  ativa BOOLEAN,
  origem ENUM('catalogo-clonada','custom','built-in') NOT NULL,
  UNIQUE KEY uk (tenant_id, codigo)
);
```

## DSL de fórmula

Gramática minimalista (BNF informal):

```
expr     := value | binary | call | conditional | parenthesized
value    := variable | literalNumber | literalConst
variable := IDENTIFIER                 -- ex: salarioBase
binary   := expr OP expr               -- OP: + - * / %
call     := IDENTIFIER '(' args? ')'   -- min, max, round, aplicaTabelaInss
conditional := 'if' '(' expr ',' expr ',' expr ')'
parenthesized := '(' expr ')'
```

Funções built-in (servidor controla):
- `min(a, b)`, `max(a, b)`, `abs(x)`, `round(x, casas)`, `floor(x)`, `ceil(x)`.
- `aplicaTabelaInss(remuneracao, competencia?)` — usa tabela vigente.
- `aplicaTabelaIrrf(base, qtdDependentes, competencia?)`.
- `diasUteis(ano, mes)`, `diasMes(ano, mes)`, `eFeriado(data, uf?, municipio?)`.

Variáveis disponíveis (preenchidas pelo engine de folha em W6 antes de avaliar):
- Funcionário: `salarioBase, qtdDependentesIrrf, qtdDependentesSf, jornadaHorasMensais`.
- Período: `competencia, diasNoMes, diasUteisMes`.
- Apontamento: `horasTrabalhadas, horasNormais, horasFalta, horasAtraso, horasExtras50, horasExtras100, horasNoturnas, diasFaltas, valorAtestados`.
- Outras rubricas (em ordem topológica): `vlr['CODIGO_RUBRICA']`.

### Sandbox de execução

`RubricaExpressionEvaluator`:
- Parser próprio (Sprache ou ANTLR4) → AST tipada.
- Validador: sem loops, sem chamadas fora do whitelist, sem alocação ilimitada.
- Interpretador puro com timeout (`100ms`).
- Resultado sempre `decimal` (folha precisa precisão).

```csharp
public sealed class RubricaExpressionEvaluator
{
    public RubricaResult Avaliar(string dsl, RubricaContexto ctx);
    public RubricaValidationResult Validar(string dsl);  // sem ctx, checa só sintaxe + whitelist
}
```

### Dependências entre rubricas

Tenant define ordem implicitamente via `dependencias_json`. Engine de folha (W6) faz topological sort:
- Se rubrica A usa `vlr['B']`, B é calculada antes.
- Ciclo (A→B→A) → erro de validação ao salvar.

## Upload de tabelas

```
POST /api/v1/admin/rh/tabelas/inss/upload
  multipart:
    arquivo: arquivo.json  (ou .csv)
    competencia: "2026-07"
    override: false

Servidor:
  ├── valida formato (schema JSON / colunas CSV)
  ├── valida soma das faixas, não-sobreposição interna, etc.
  ├── inicia transação:
  │   ├── fecha vigência anterior (UPDATE ... SET competencia_fim = "2026-06")
  │   └── INSERT linhas novas com competencia_inicio="2026-07"
  ├── invalida cache redis "tabela:inss:*"
  └── retorna { totalInseridas, vigenciaAnteriorFechada, alertas }
```

Override flag:
- `false` (default): exige próxima competência (não permite refazer passado).
- `true`: permite reescrever vigência sobreposta (auditado pesado).

## Formato de upload por tipo

### INSS (JSON)
```json
{
  "tipo": "inss",
  "competenciaInicio": "2026-07",
  "faixas": [
    { "ordem": 1, "inicio": 0,        "fim": 1518.00,   "aliquotaPct": 7.5,  "parcelaDeduzir": 0 },
    { "ordem": 2, "inicio": 1518.01,  "fim": 2793.88,   "aliquotaPct": 9.0,  "parcelaDeduzir": 22.77 },
    { "ordem": 3, "inicio": 2793.89,  "fim": 4190.83,   "aliquotaPct": 12.0, "parcelaDeduzir": 106.59 },
    { "ordem": 4, "inicio": 4190.84,  "fim": 8157.41,   "aliquotaPct": 14.0, "parcelaDeduzir": 190.40 }
  ]
}
```

### CSV alternativo (mesmo conteúdo)
```csv
ordem,inicio,fim,aliquota_pct,parcela_deduzir
1,0,1518.00,7.5,0
2,1518.01,2793.88,9.0,22.77
...
```

### IRRF, FGTS, SM, salário-família — schemas análogos documentados em `documentacao/rh/uploads-tabelas-formato.md`.

## Cache

```
TabelaInssRepository.ObterVigenteAsync(competencia):
  cacheKey = $"tabela:inss:{competencia}"
  if redis.HasKey(cacheKey): return redis.Get(cacheKey)
  result = await dbQuery(...)
  redis.Set(cacheKey, result, ttl: 1h)
  return result
```

Invalidação:
- Cada upload publica evento `TabelasLegaisAtualizadas`.
- Listener invalida `tabela:inss:*` no Redis.

## Permissions

- `Recursos.AdminTabelasLegais` × `Acoes.Upload, Listar` → apenas Root e RhAdmin.
- `Recursos.RhRubrica` × CRUD + `Testar` → RH.

## Test strategy

- Unit: parser DSL — 30 expressões válidas + 20 inválidas (loop, eval, chamada não whitelistada).
- Unit: evaluator — 50 expressões com contextos fixture → resultados conferidos contra cálculo manual.
- Unit: upload INSS — JSON válido, JSON com sobreposição, CSV com erro de coluna.
- Integration: upload nova competência fecha anterior atomicamente.
- Integration: cache invalida após upload.
