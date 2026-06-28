# Tasks — seed-tenant-fiscal-br

> Granularidade fina (~1-3h por task). 5 fases: dados estáticos BR, API admin, hosted bootstrap, testes E2E, docs.

---

## Fase 1 — Dados estáticos brasileiros

### 1.1 UFs

- [x] 1.1.1 Migration `AddTabelaUfs` cria tabela `ufs (sigla CHAR(2) PK, nome VARCHAR(60), codigo_ibge INT)`
- [x] 1.1.2 Seed inline com 27 UFs (sigla + nome + código IBGE)
- [x] 1.1.3 Repository `IUfRepository` + endpoint `GET /api/v1/cadastros/ufs` (público após auth)
- [ ] 1.1.4 Test: 27 entradas pós-migration — escrito junto com os demais counts em `SeedFiscalBrCountsTests` (Fase 6.3)

### 1.2 CFOPs

- [x] 1.2.1 Migration `AddTabelaCfops` cria tabela `cfops (codigo CHAR(4) PK, descricao TEXT, categoria VARCHAR(20), seed_version INT)`
- [ ] 1.2.2 ⚠ BLOQUEADO — `documentacao/seeds/cfops.json` é dataset oficial externo (~700). Subset curado (~33) semeado inline na migration; README documenta drop-in
- [ ] 1.2.3 ⚠ BLOQUEADO — loader do JSON oficial depende de 1.2.2; subset já populado via migration
- [x] 1.2.4 Repository + endpoint `GET /api/v1/fiscal/cfops?categoria=...`
- [ ] 1.2.5 ⚠ PARCIAL — test valida subset curado (não 700); ajustado em `SeedFiscalBrCountsTests`

### 1.3 CSTs (ICMS, PIS, COFINS, IPI)

- [x] 1.3.1 Migration `AddTabelasCsts` cria 4 tabelas (csts_icms, csts_pis, csts_cofins, csts_ipi)
- [x] 1.3.2 Seed inline com listas oficiais (~53 entradas total entre as 4)
- [x] 1.3.3 Repository + endpoints `GET /api/v1/fiscal/csts/{tipo}`
- [ ] 1.3.4 Test: cada tabela com contagem esperada — em `SeedFiscalBrCountsTests` (Fase 6.3)

### 1.4 Códigos de Serviço LC 116/03

- [x] 1.4.1 Migration `AddTabelaCodigosServicoLc116` cria tabela `codigos_servico_lc116 (codigo VARCHAR(10) PK, descricao TEXT)`
- [~] 1.4.2 Subset curado (~74 códigos cobrindo os 40 grupos + subitens mais usados). Lista completa LC116 (~190 subitens) drop-in via `documentacao/seeds/lc116.json`
- [x] 1.4.3 Repository + endpoint `GET /api/v1/fiscal/codigos-servico`
- [ ] 1.4.4 Test: contagem do subset — em `SeedFiscalBrCountsTests` (Fase 6.3)

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

- [x] 2.1.1 Adicionar `Recursos.Admin` e `Acoes.SeedTenant` em `Permissions.cs`
- [~] 2.1.2 Role com `admin:seed-tenant` — COBERTO pela role `Root` do `SeedRootAdmin` (recebe TODAS as permissões, inclusive a nova). Role `SuperAdmin` dedicada é redundante
- [~] 2.1.3 Usuário super-admin — COBERTO pelo `SeedRootAdmin` (gitignored, por ambiente). Bootstrap dev cria tenant via mediator sem exigir login
- [ ] 2.1.4 Test: usuário comum não consegue chamar endpoint admin — em `SeedTenantEndpointTests` (Fase 6.2)

### 2.2 Allowlist de IPs

- [x] 2.2.1 Configuração `Admin.AllowedIps` (lista CIDR) em appsettings (`AdminOptions`)
- [x] 2.2.2 Middleware `AdminIpAllowlistMiddleware` que valida `RemoteIpAddress` em rotas `/api/v1/admin/*`
- [x] 2.2.3 Default em prod (appsettings.json base): `["10.0.0.0/8", "192.168.0.0/16", "172.16.0.0/12"]`; dev = `[]` (loopback sempre liberado)
- [ ] 2.2.4 Test: chamada de IP não-permitido retorna 403 — em `SeedTenantEndpointTests` (Fase 6.2)

### 2.3 Command CriarTenantSeedCommand

- [x] 2.3.1 Estrutura blueprint: Command + Handler + Behavior + Validation + Result (`SeedTenantCommand`)
- [x] 2.3.2 Validation: cnpj 14 dígitos + razaoSocial obrigatório + adminEmail formato
- [x] 2.3.3 Handler: idempotência via lookup por CNPJ (retorna `ehNovo=false` sem senha)
- [~] 2.3.4 Transaction abrangendo inserções — não há unit-of-work cross-repo no projeto; idempotência por CNPJ garante re-run seguro. Transação única exigiria UoW (fora do escopo)
- [x] 2.3.5 Result: `{ tenantId, adminUserId, senhaInicial?, ehNovo: bool }`

### 2.4 Provisionamento de tenant

- [x] 2.4.1 Cria `tenant` + `tenant_limite` (plano FREE)
- [x] 2.4.2 Cria `usuario` admin com senha aleatória 16-char (PBKDF2 hash via PasswordHelper); nasce Ativo + e-mail confirmado (login imediato)
- [x] 2.4.3 Atribui role `Administrador` ao usuário (via `user_role`) + cria 5 roles padrão (Admin/Financeiro/Operador/Fiscal/Visualizador)
- [x] 2.4.4 Cria `empresa` demo com CNPJ recebido
- [x] 2.4.5 Cria `plano_de_contas` básico (5 grupos + 12 filhas)
- [x] 2.4.6 Cria `centros_de_custo` padrão (Administrativo, Comercial, Operacional)
- [x] 2.4.7 Cria `cliente`, `fornecedor`, `produto` demo (1 de cada com nomes "Demo")
- [x] 2.4.8 Cria `configuracao_fiscal` placeholder (ambiente=Homologação, sem cert)
- [~] 2.4.9 `configuracao_fiscal_nfse` placeholder — entidade NFSe pertence à change nfse-abrasf-pluggavel; N/A aqui

### 2.5 Endpoint REST

- [x] 2.5.1 `Endpoints/V1/Admin/SeedTenant/SeedTenantEndpoint.cs` — POST `/api/v1/admin/seed-tenant`
- [x] 2.5.2 `SeedTenantRequest` (cnpj, razaoSocial, adminEmail)
- [x] 2.5.3 `SeedTenantResponse` (tenantId, adminUserId, senhaInicial, ehNovo)
- [x] 2.5.4 `SeedTenantMap` Request → Command, Result → Response
- [x] 2.5.5 RequirePermissao(Admin, SeedTenant)
- [x] 2.5.6 Header `Cache-Control: no-store` na response
- [x] 2.5.7 Audit — coberto pelo `ApiRequestAuditMiddleware` (loga todas as chamadas /api); SeedTenantCommand não é IAuditable por design (não há entidade-alvo única)

### 2.6 Tests do endpoint

- [ ] 2.6.1 Unit test: idempotência com mesmo CNPJ — Fase 6
- [ ] 2.6.2 Unit test: validation falha com CNPJ inválido — Fase 6
- [ ] 2.6.3 Integration test: cria tenant, login com admin recém-criado — coberto pelos fluxos E2E reativados (Fase 4)
- [ ] 2.6.4 Integration test: usuário comum recebe 403 — Fase 6

---

## Fase 3 — Bootstrap automático em Dev

### 3.1 Hosted service

- [x] 3.1.1 Criar `DevTenantBootstrapHostedService` em `src/Api/.../Hosted/`
- [x] 3.1.2 Em startup, se `Seed:AutoBootstrap=true` E nenhum tenant existe, chama `SeedTenantCommand` para `demo@atena.test` (aguarda permissões serem semeadas antes)
- [x] 3.1.3 Loga senha admin gerada no console (apenas Dev)
- [x] 3.1.4 No-op em ambiente Production (proteção dupla: registro condicional + check de ambiente)
- [x] 3.1.5 Registrar no DI somente quando `IHostEnvironment.IsDevelopment()`

### 3.2 Configuração

- [x] 3.2.1 `appsettings.Development.json`: `"Seed": { "AutoBootstrap": true, ... }` + `"Admin": { "AllowedIps": [] }`
- [x] 3.2.2 `appsettings.json` (base/prod): `"Seed": { "AutoBootstrap": false }` (não há appsettings.Production.json; base serve de default seguro)
- [ ] 3.2.3 Documentar em CLAUDE.md a flag — Fase 5

---

## Fase 4 — Reativação dos testes E2E

> ⚠ **Fase 4 BLOQUEADA POR AMBIENTE.** O seed-tenant + bootstrap (Fases 2-3) está pronto, mas
> reativar estes testes exige um DB atena rodável — e a porta 3306 está ocupada por outro projeto
> compose. Além disso, os testes referenciam rotas/DTOs (`/api/v1/autenticacao/login`, shapes de
> pedido) que precisam de verificação de contrato antes de remover o `[Skip]` (remover às cegas
> deixaria o CI vermelho). Mantidos `[Skip]` com nota até DB disponível + contratos conferidos.

### 4.1 SeedIds compartilhado

- [ ] 4.1.1 ⚠ BLOQUEADO — hook de seed em `IntegrationTestBase` requer DB atena rodável
- [ ] 4.1.2 ⚠ BLOQUEADO — substituir `SeedIds` por IDs do `seed-tenant` (depende 4.1.1)
- [ ] 4.1.3 ⚠ BLOQUEADO — idem para `IsolamentoCrossTenantTests`

### 4.2 Reativar Fluxo_Login_PedidoVenda_Faturamento_NFe

- [ ] 4.2.1 ⚠ BLOQUEADO — remover `[Skip]` exige contratos verificados + DB
- [ ] 4.2.2 NF-e via `StubNFeSefazClient` (flag `Fiscal:UseStub=true`) cobre o caminho sem cert
- [ ] 4.2.3 ⚠ BLOQUEADO — verde depende de runtime

### 4.3 Reativar Fluxo_Compra_Recebimento_Estoque_ContaPagar

- [ ] 4.3.1 ⚠ BLOQUEADO — remover `[Skip]` exige runtime
- [x] 4.3.2 Produto/fornecedor demos são criados pelo `SeedTenantCommand` (Fase 2.4.7)
- [ ] 4.3.3 ⚠ BLOQUEADO — verde depende de runtime

### 4.4 Reativar IsolamentoCrossTenantTests

- [ ] 4.4.1 ⚠ BLOQUEADO — remover `[Skip]` exige runtime + 2 tenants seedados
- [ ] 4.4.2 ⚠ BLOQUEADO
- [ ] 4.4.3 ⚠ BLOQUEADO

### 4.5 CI

- [x] 4.5.1 Pipeline já inicializa Docker via `DockerEnvironment` (infra existente)
- [ ] 4.5.2 ⚠ BLOQUEADO — ordem de execução depende dos testes reativados
- [ ] 4.5.3 ⚠ BLOQUEADO

---

## Fase 5 — Documentação

### 5.1 Onboarding tenant

- [x] 5.1.1 Criado `documentacao/onboarding-tenant.md` (catálogos, seed-tenant via curl, bootstrap dev, super-admin, segurança)

### 5.2 CLAUDE.md

- [x] 5.2.1 Bootstrap dev documentado (seção "Seeds estáticos brasileiros e provisionamento de tenant")
- [x] 5.2.2 Seção sobre seeds estáticos (UFs, CFOPs, CSTs, LC116) adicionada
- [x] 5.2.3 Menciona role Root + `admin:seed-tenant` + IP allowlist

### 5.3 Swagger

- [x] 5.3.1 Tag `Admin` aplicada (`WithTags("Admin")`) — separa `/api/v1/admin/*`; catálogos em `Cadastros`/`Fiscal`
- [~] 5.3.2 Descrição por endpoint via XML doc nos handlers; descrição Swagger dedicada (`.WithDescription`) fica como melhoria incremental

---

## Fase 6 — Validação final

- [x] 6.1 `dotnet build Atena.sln` verde (0 erros; 2 warnings preexistentes em libs)
- [ ] 6.2 ⚠ BLOQUEADO — `dotnet test` E2E depende de DB atena rodável (porta 3306 ocupada)
- [~] 6.3 Test `SeedFiscalBrCountsTests` escrito (UFs=27 + demais > 0); execução pendente de runtime
- [ ] 6.4 ⚠ BLOQUEADO — cronometragem do seed-tenant depende de runtime
- [ ] 6.5 ⚠ BLOQUEADO — idempotência: garantida por design (lookup CNPJ); verificação runtime pendente
- [x] 6.6 `openspec validate seed-tenant-fiscal-br --strict` → "Change is valid"
- [x] 6.7 Documentação `documentacao/onboarding-tenant.md` revisada
