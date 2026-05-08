# Tasks — seed-tenant-fiscal-br

> Granularidade fina (~1-3h por task). 5 fases: dados estáticos BR, API admin, hosted bootstrap, testes E2E, docs.

---

## Fase 1 — Dados estáticos brasileiros

### 1.1 UFs

- [ ] 1.1.1 Migration `AddTabelaUfs` cria tabela `ufs (sigla CHAR(2) PK, nome VARCHAR(60), codigo_ibge INT)`
- [ ] 1.1.2 Seed inline com 27 UFs (sigla + nome + código IBGE)
- [ ] 1.1.3 Repository `IUfRepository` + endpoint `GET /api/v1/cadastros/ufs` (público após auth)
- [ ] 1.1.4 Test: 27 entradas pós-migration

### 1.2 CFOPs

- [ ] 1.2.1 Migration `AddTabelaCfops` cria tabela `cfops (codigo CHAR(4) PK, descricao TEXT, categoria VARCHAR(20), seed_version INT)`
- [ ] 1.2.2 Adicionar `documentacao/seeds/cfops.json` com lista oficial Receita Federal (~700 entradas)
- [ ] 1.2.3 Migration carrega o JSON e popula tabela
- [ ] 1.2.4 Repository + endpoint `GET /api/v1/fiscal/cfops?categoria=...`
- [ ] 1.2.5 Test: pelo menos 700 entradas pós-migration

### 1.3 CSTs (ICMS, PIS, COFINS, IPI)

- [ ] 1.3.1 Migration `AddTabelasCsts` cria 4 tabelas (csts_icms, csts_pis, csts_cofins, csts_ipi)
- [ ] 1.3.2 Seed inline com listas oficiais (~40 entradas total entre as 4)
- [ ] 1.3.3 Repository + endpoints `GET /api/v1/fiscal/csts/{tipo}`
- [ ] 1.3.4 Test: cada tabela com contagem esperada

### 1.4 Códigos de Serviço LC 116/03

- [ ] 1.4.1 Migration `AddTabelaCodigosServicoLc116` cria tabela `codigos_servico_lc116 (codigo VARCHAR(10) PK, descricao TEXT)`
- [ ] 1.4.2 Seed inline com 123 códigos da LC 116/03
- [ ] 1.4.3 Repository + endpoint `GET /api/v1/fiscal/codigos-servico`
- [ ] 1.4.4 Test: 123 entradas pós-migration

### 1.5 NCMs (opt-in)

- [ ] 1.5.1 Migration `AddTabelaNcms` cria tabela `ncms (codigo CHAR(8) PK, descricao TEXT)` — vazia por default
- [ ] 1.5.2 Adicionar `documentacao/seeds/ncms.json.gz` (compactado, ~10k entradas, lista TIPI)
- [ ] 1.5.3 Endpoint admin `POST /api/v1/admin/seed-fiscal-br/ncms` que carrega e popula (idempotente)
- [ ] 1.5.4 Feature flag `Seed.LoadNcmsOnStartup=false` (default); se `true`, hosted service carrega automaticamente
- [ ] 1.5.5 Test: endpoint carrega 10k+ entradas em < 30s

### 1.6 Municípios IBGE (opt-in)

- [ ] 1.6.1 Migration `AddTabelaMunicipios` cria tabela `municipios (codigo_ibge INT PK, nome VARCHAR(120), uf CHAR(2))`
- [ ] 1.6.2 Adicionar `documentacao/seeds/municipios.json.gz` (~5570 entradas)
- [ ] 1.6.3 Endpoint admin `POST /api/v1/admin/seed-fiscal-br/municipios` (idempotente)
- [ ] 1.6.4 Feature flag `Seed.LoadMunicipiosOnStartup=false`
- [ ] 1.6.5 Test: carrega 5570+ entradas

### 1.7 Endpoint agregador

- [ ] 1.7.1 `POST /api/v1/admin/seed-fiscal-br?incluir=ncm,municipios` — chama os opt-ins de uma vez
- [ ] 1.7.2 Resposta: `{ ufs: 27, cfops: 712, csts: 40, lc116: 123, ncms: 10234, municipios: 5570 }`
- [ ] 1.7.3 Test E2E

---

## Fase 2 — API admin de seed-tenant

### 2.1 Permissão e role SuperAdmin

- [ ] 2.1.1 Adicionar `Recursos.Admin` e `Acoes.SeedTenant` em `Permissions.cs`
- [ ] 2.1.2 Migration: criar role `SuperAdmin` global (sem tenant_id) com permissão `Admin.SeedTenant`
- [ ] 2.1.3 Seed inicial: criar 1 usuário SuperAdmin com email do .env e senha do .env (apenas em ambiente Dev/Stg)
- [ ] 2.1.4 Test: usuário comum não consegue chamar endpoint admin

### 2.2 Allowlist de IPs

- [ ] 2.2.1 Configuração `Admin.AllowedIps` (lista CIDR) em appsettings
- [ ] 2.2.2 Middleware `AdminIpAllowlistMiddleware` que valida `RemoteIpAddress` em rotas `/api/v1/admin/*`
- [ ] 2.2.3 Default em prod: `["10.0.0.0/8", "192.168.0.0/16"]`
- [ ] 2.2.4 Test: chamada de IP não-permitido retorna 403

### 2.3 Command CriarTenantSeedCommand

- [ ] 2.3.1 Estrutura blueprint: Command + Handler + Behavior + Validation + Result
- [ ] 2.3.2 Validation: cnpj formato + razaoSocial obrigatório + adminEmail formato
- [ ] 2.3.3 Handler: idempotência via lookup por CNPJ
- [ ] 2.3.4 Handler: transaction abrangendo todas as inserções
- [ ] 2.3.5 Result: `{ tenantId, adminUserId, senhaInicial?, ehNovo: bool }`

### 2.4 Provisionamento de tenant

- [ ] 2.4.1 Cria `tenant` + `tenant_limite` (default plano Trial)
- [ ] 2.4.2 Cria `usuario` admin com senha aleatória 16-char (BCrypt hash)
- [ ] 2.4.3 Atribui role `Admin` ao usuário (via `user_role`)
- [ ] 2.4.4 Cria `empresa` demo com CNPJ recebido
- [ ] 2.4.5 Cria `plano_de_contas` básico (5 grupos, ~30 contas) — seed embutido
- [ ] 2.4.6 Cria `centros_de_custo` padrão (Administrativo, Comercial, Operacional)
- [ ] 2.4.7 Cria `cliente`, `fornecedor`, `produto` demo (1 de cada com nomes "Demo")
- [ ] 2.4.8 Cria `configuracao_fiscal` placeholder (ambiente=Homologação, sem cert)
- [ ] 2.4.9 Cria `configuracao_fiscal_nfse` placeholder

### 2.5 Endpoint REST

- [ ] 2.5.1 `Endpoints/V1/Admin/SeedTenant/SeedTenantEndpoint.cs` — POST `/api/v1/admin/seed-tenant`
- [ ] 2.5.2 `SeedTenantRequest` (cnpj, razaoSocial, adminEmail)
- [ ] 2.5.3 `SeedTenantResponse` (tenantId, adminUserId, senhaInicial, ehNovo)
- [ ] 2.5.4 `SeedTenantMap` Request → Command, Result → Response
- [ ] 2.5.5 RequirePermissao(Admin, SeedTenant)
- [ ] 2.5.6 Header `Cache-Control: no-store` na response
- [ ] 2.5.7 Audit log obrigatório

### 2.6 Tests do endpoint

- [ ] 2.6.1 Unit test: idempotência com mesmo CNPJ
- [ ] 2.6.2 Unit test: validation falha com CNPJ inválido
- [ ] 2.6.3 Integration test: cria tenant, login com admin recém-criado, valida acesso a recursos
- [ ] 2.6.4 Integration test: usuário comum recebe 403

---

## Fase 3 — Bootstrap automático em Dev

### 3.1 Hosted service

- [ ] 3.1.1 Criar `DevTenantBootstrapHostedService` em `src/Api/.../Hosted/`
- [ ] 3.1.2 Em startup, se `Seed.AutoBootstrap=true` E nenhum tenant existe, chama `CriarTenantSeedCommand` para `demo@atena.test`
- [ ] 3.1.3 Loga senha admin gerada no console (apenas Dev)
- [ ] 3.1.4 No-op em ambiente Production (proteção dupla)
- [ ] 3.1.5 Registrar no DI somente quando `IHostEnvironment.IsDevelopment()`

### 3.2 Configuração

- [ ] 3.2.1 `appsettings.Development.json`: `"Seed": { "AutoBootstrap": true, "LoadNcmsOnStartup": false, "LoadMunicipiosOnStartup": false }`
- [ ] 3.2.2 `appsettings.Production.json`: `"Seed": { "AutoBootstrap": false }`
- [ ] 3.2.3 Documentar em CLAUDE.md a flag

---

## Fase 4 — Reativação dos testes E2E

### 4.1 SeedIds compartilhado

- [ ] 4.1.1 Em `IntegrationTestBase`, sobrescrever fixture: chama `seed-tenant` na primeira execução, captura IDs reais
- [ ] 4.1.2 Substituir `SeedIds` hardcoded em `FluxoVendaCompletaTests` pelos IDs do tenant criado
- [ ] 4.1.3 Mesmo para `IsolamentoCrossTenantTests` — criar 2 tenants

### 4.2 Reativar Fluxo_Login_PedidoVenda_Faturamento_NFe

- [ ] 4.2.1 Remover `[Skip = "..."]`
- [ ] 4.2.2 Garantir que stub NF-e (ou cliente real, dependendo do timing das changes) responde com sucesso em homolog
- [ ] 4.2.3 Test verde

### 4.3 Reativar Fluxo_Compra_Recebimento_Estoque_ContaPagar

- [ ] 4.3.1 Remover `[Skip = "..."]`
- [ ] 4.3.2 Garantir que produto/fornecedor demos foram seeded
- [ ] 4.3.3 Test verde

### 4.4 Reativar IsolamentoCrossTenantTests

- [ ] 4.4.1 Remover `[Skip]`
- [ ] 4.4.2 Validar que tenant1 não vê dados de tenant2 em todos os recursos críticos
- [ ] 4.4.3 Test verde

### 4.5 CI

- [ ] 4.5.1 Garantir que pipeline integration test inicializa Docker (já existe `DockerEnvironment`)
- [ ] 4.5.2 Pipeline executa `RouteSnapshotTests` + `Fluxo*Tests` + `Isolamento*Tests` em ordem
- [ ] 4.5.3 Tempo total < 5 min

---

## Fase 5 — Documentação

### 5.1 Onboarding tenant

- [ ] 5.1.1 Criar `documentacao/onboarding-tenant.md` com:
  - Pré-requisitos (DB up, migrations rodadas, seeds estáticos)
  - Passo a passo `POST /api/v1/admin/seed-tenant` com curl
  - Como configurar cert fiscal pós-criação
  - Troubleshooting

### 5.2 CLAUDE.md

- [ ] 5.2.1 Atualizar seção "Build & Run" com instrução de bootstrap dev
- [ ] 5.2.2 Adicionar seção sobre seeds estáticos (UFs, CFOPs, etc.)
- [ ] 5.2.3 Mencionar SuperAdmin role + IP allowlist

### 5.3 Swagger

- [ ] 5.3.1 Tag `Admin` separada para endpoints `/api/v1/admin/*`
- [ ] 5.3.2 Descrição clara em cada endpoint sobre permissão necessária e idempotência

---

## Fase 6 — Validação final

- [ ] 6.1 `dotnet build Atena.sln` verde
- [ ] 6.2 `dotnet test` (unit + integration) verde — incluindo os 3 testes E2E reativados
- [ ] 6.3 Migration aplicada em DB limpo gera contagens esperadas (UFs=27, CFOPs≥700, CSTs≥40, LC116=123)
- [ ] 6.4 Endpoint `seed-tenant` cria tenant funcional em < 5s (cronometrado em test)
- [ ] 6.5 Idempotência confirmada (chamada repetida não duplica)
- [ ] 6.6 `openspec validate seed-tenant-fiscal-br --strict` verde
- [ ] 6.7 Documentação em `documentacao/onboarding-tenant.md` revisada
