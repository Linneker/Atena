# Seeds de dados estáticos brasileiros

Esta pasta hospeda os datasets oficiais **volumosos** usados pelo change `seed-tenant-fiscal-br`.
Eles **não** são versionados no git por tamanho/origem externa, e são carregados sob demanda.

## Status dos datasets

| Dataset | Arquivo esperado | Volume | Status | Fonte oficial |
|---------|------------------|--------|--------|---------------|
| UFs | _(inline na migration)_ | 27 | ✅ Completo | IBGE |
| CSTs ICMS/PIS/COFINS/IPI | _(inline na migration)_ | ~40 | ✅ Completo | RICMS / Receita Federal |
| Códigos LC 116/03 | _(inline na migration)_ | 123 | ✅ Completo | Lei Complementar 116/2003 |
| CFOPs | `cfops.json` | ~700 | ⚠️ **SUBSET** (≈33 curados na migration) | [Receita — Anexo CFOP](https://www.gov.br/) |
| NCMs | `ncms.json.gz` | ~10.000 | ⚠️ **BLOQUEADO** (tabela vazia) | [Receita — TIPI](https://www.gov.br/receitafederal) |
| Municípios IBGE | `municipios.json.gz` | ~5.570 | ⚠️ **BLOQUEADO** (tabela vazia) | [IBGE — DTB](https://www.ibge.gov.br/) |
| CBOs (rh-fundacao) | `cbo.json` | ~2.500 | ⚠️ **SAMPLE** (10 ocupações em `cbo.json`) | [Ministério do Trabalho — CBO](https://www.gov.br/trabalho-e-emprego/pt-br/assuntos/trabalhador/cbo) |

## Como completar os datasets bloqueados

Os catálogos volumosos têm a **máquina pronta** (tabela + repositório + endpoint + loader),
mas dependem do arquivo de dados oficial. Para popular:

### CFOPs (`cfops.json`)
Formato: `[{ "codigo": "5102", "descricao": "...", "categoria": "Saida" }, ...]`
Hoje a migration `V20260514002_AddTabelaCfops` semeia um subset curado dos mais usados.
Para a lista completa, coloque `cfops.json` aqui e rode o loader admin (a ser plugado em 1.2.3).

### NCMs (`ncms.json.gz`)
Formato (gzip de JSON): `[{ "codigo": "01012100", "descricao": "..." }, ...]`
Endpoint admin: `POST /api/v1/admin/seed-fiscal-br/ncms` (idempotente).
Default não carrega no boot (`Seed:LoadNcmsOnStartup=false`).

### Municípios (`municipios.json.gz`)
Formato (gzip de JSON): `[{ "codigoIbge": 3550308, "nome": "São Paulo", "uf": "SP" }, ...]`
Endpoint admin: `POST /api/v1/admin/seed-fiscal-br/municipios` (idempotente).
Default não carrega no boot (`Seed:LoadMunicipiosOnStartup=false`).

### CBOs (`cbo.json`)
Formato: `[{ "codigo": "252305", "titulo": "...", "grandeGrupo": "2", "familia": "2523" }, ...]`
Tabela `cbos` é nacional (não tenant-scoped), código de 6 dígitos é a PK natural.
Hoje há uma amostra de 10 ocupações comuns em `cbo.json` para popular dropdown em telas RH.
Para a lista completa (~2.500), substitua o arquivo e POST para `/api/v1/admin/rh/cbos/seed`
(upsert idempotente, permissão `admin:seed-tenant`, exclusiva do Root).

> **Para a demo "rodável fim-a-fim"** esses datasets não são necessários — os fluxos E2E
> (Compra→Estoque, Venda→Faturamento→NFe-stub) não dependem deles. Eles enriquecem os
> dropdowns das telas fiscais.
