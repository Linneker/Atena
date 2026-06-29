# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 📚 Knowledge Base RAG — leia primeiro

Antes de mexer em qualquer feature, **consulte o índice da knowledge base**:
**[`documentacao/rag/INDEX.md`](documentacao/rag/INDEX.md)**

Estrutura: 1 arquivo por funcionalidade (plataforma, cadastros, financeiro,
estoque, compras, vendas, fiscal-nfe, rh-fundacao-w1, rh-ponto-interno-w2,
rh-mobile-w3, rh-ponto-oficial-671-w4, frontend-angular, mobile-maui,
auditoria-observabilidade, infraestrutura). Cada arquivo é auto-contido com
entidades, endpoints, handlers, decisões e paths concretos.

**Regra de manutenção**: toda PR que altera uma funcionalidade DEVE atualizar
o arquivo `documentacao/rag/<funcionalidade>.md` correspondente — o checklist
da PR (`.github/pull_request_template.md`) tem item dedicado. Sem essa
disciplina, o RAG envelhece e queries semânticas vão dar respostas erradas.

## Project Overview

**Atena** is a multi-tenant ERP (Enterprise Resource Planning) covering financial management, sales, purchases, inventory, fiscal (NF-e), reports and configuration. The system has a .NET 8 Minimal API backend and a single Angular 17 frontend.

## Build & Run Commands

### Backend (.NET 8)

```powershell
# Build entire solution
dotnet build Atena.sln

# Run the API (from src/Api/Acme.Sistemas.Atena.Api/)
dotnet run --project src/Api/Acme.Sistemas.Atena.Api/Acme.Sistemas.Atena.Api.csproj

# Run tests
dotnet test

# EF Core migrations
dotnet ef migrations add <MigrationName> --project src/Data/Acme.Sistemas.Infrastructure
dotnet ef database update --project src/Data/Acme.Sistemas.Infrastructure
```

### Frontend (Angular 17) — single app at `site/atena-web/`

```powershell
cd site/atena-web

# Install dependencies
npm install

# Development server (http://localhost:4200)
npm start

# Production build
npm run build:prod

# Run tests
npm test
```

### Docker

```powershell
# Sobe a stack completa (API, MySQL, Redis, RabbitMQ, MinIO).
# Compose vive em infra/compose/ — paths relativos consideram raiz do repo.
docker compose -f infra/compose/docker-compose.yml up -d

# Build da imagem da API (Dockerfile em src/Api/.../Dockerfile, build context na raiz)
docker build -t atena-api -f src/Api/Acme.Sistemas.Atena.Api/Dockerfile .
```

### Kubernetes

Manifests em `infra/k8s/v1/` (namespace, configmap, deployment, service).
Cluster local com `kind` configurado em `infra/k8s/kind-config.yaml`
(3 control-plane + 3 worker, port-mapping 30000→5000).

```powershell
# Cria cluster kind local
kind create cluster --name atena --config infra/k8s/kind-config.yaml

# Aplica manifests
kubectl apply -f infra/k8s/v1/

# Ou use o script automatizado (build + load + apply + wait)
pwsh infra/k8s/v1/deploy-kind.ps1
```

## Architecture

Clean Architecture com camadas bem definidas e Mediator próprio:

```
src/
├── Api/
│   └── Acme.Sistemas.Atena.Api          → Minimal API, Endpoints/V1/, IEndpoint
├── Service/
│   ├── Acme.Sistemas.Services           → Handlers (Command/Query), V1/
│   ├── Acme.Sistemas.Domain             → Entidades, Enums, Interfaces/Repository
│   └── Acme.Sistemas.Core               → Mediator próprio, IRequest/Handler, helpers (Hash, Jwt, Password), Permissions consts
└── Data/
    ├── Acme.Sistemas.Repository         → Repositórios SQL puros
    ├── Acme.Sistemas.Infrastructure     → DbContext, MigrationRunner, Cache (Redis), Email, RabbitMQ, GED (S3/local)
    └── Acme.Sistemas.ExternalIntegration → HttpClientProxy, ViaCEP, integrações externas

test/
├── Unit/                                 → Acme.Sistemas.Services.UnitTest (xUnit + Moq + Bogus)
└── Integration/                          → Acme.Sistemas.IntegrationTest (WebApplicationFactory + Docker)

infra/
├── docker-compose.yml                    → API + MySQL + Redis + RabbitMQ + MinIO
└── k8s/v1/                               → Deployment, Service, ConfigMap
```

Dependencies flow inward: `Api → Services → Domain ← Repository ← Infrastructure`.

### Blueprint Acme

A organização técnica segue o blueprint comum dos projetos Acme. Documentos canônicos:

- `documentacao/blueprint.yml` (norma técnica)
- `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md` (guia)
- `documentacao/templates/` — templates de Behavior, Result, Validation, Endpoint, Request, Response, Map

**Justificativa "Domain por módulo ERP":** o blueprint é norma de organização (CQRS, layout, infra, cache); o modelo de domínio é específico do projeto. Atena mantém Domain organizado por área ERP (Financeiro, Estoque, Compras, etc.) — isto é decisão deliberada, **não** divergência do blueprint.

#### Como criar um novo Endpoint no padrão

```
Api/Endpoints/V1/{Recurso}/{Verbo}{Recurso}/
├── {Verbo}{Recurso}Endpoint.cs    ← classe IEndpoint que registra a rota
├── {Verbo}{Recurso}Request.cs     ← DTO de entrada
├── {Verbo}{Recurso}Response.cs    ← DTO de saída
└── {Verbo}{Recurso}Map.cs         ← extensions Request → Command, Result → Response
```

Implementa `IEndpoint`, descoberto via reflexão por `EndpointRegistrationExtensions`. Use `RequirePermissao(Permissions.Of(Recursos.X, Acoes.Y))`.

**100% de aderência:** todas as ~120 rotas /api/v1 seguem este padrão (uma rota por pasta, 4 arquivos). Validado em runtime por `EndpointConventionTests` (projeto Integration), que itera `EndpointDataSource.Endpoints` e exige siblings em cada pasta. Allow-list cobre apenas `/health`. Não existem mais arquivos `*Endpoints.cs` (plural) em `Endpoints/V1/`.

#### Como criar um novo Command com Behavior+Result

```
Service/Acme.Sistemas.Services/V1/{Funcionalidade}/Command/{Acao}/
├── {Acao}Command.cs               ← record + IRequest<ResponseDefault<{Acao}CommandResult>>
├── {Acao}CommandHandler.cs        ← IRequestHandler<…>; lógica de negócio
├── {Acao}CommandBehavior.cs       ← IPipelineBehavior<…>; cache invalidation, regra extra
├── {Acao}CommandResult.cs         ← record imutável (payload de saída)
└── {Acao}CommandValidation.cs     ← AbstractValidator<…> com FluentValidation
```

O pipeline transversal aplica em ordem: **Validation → CacheLookup → Audit → Log → Behavior específico → Handler**. Os 4 transversais vivem em `Acme.Sistemas.Core/Mediators/Behaviors/`. Convenções validadas em CI por `ConvencoesBlueprintTests`.

### Key Patterns

- **CQRS via Mediator próprio** (em `Acme.Sistemas.Core/Mediators/`): Commands em `V1/<Recurso>/Command/`, Queries em `V1/<Recurso>/Query/` — cada um com `Command|Query`, `Handler`, `Behavior`, `Result`, `Validation`
- **Repository Pattern com SQL puro**: Sem ORM no Read; queries em `<Recurso>Query.cs`
- **Multi-tenancy**: `tenant_id` em todas as tabelas; `ITenantContext` (scoped) injetado pelo `TenantMiddleware` extraindo do JWT
- **RBAC**: `roles`, `permissions`, `role_permissions`, `user_roles`. `PermissaoAttribute`/policies validam claims do JWT. Permissões em `Acme.Sistemas.Core/Const/Permissions.cs`
- **JWT + Refresh + Blacklist**: Login retorna access (claims com permissões) + refresh; logout joga refresh na `token_blacklist`
- **Auditoria**: `AuditBehavior` no pipeline (antes/depois) + `ApiRequestAuditMiddleware`
- **NF-e assíncrona**: Emissão via fila RabbitMQ; worker `NFeTransmissaoWorker` consome e transmite à SEFAZ; XMLs no S3 (`{tenant_id}/{ano}/{mes}/{chave}.xml`); contingência SVRS automática
- **Cliente SEFAZ próprio**: `RealNFeSefazClient` em `Acme.Sistemas.ExternalIntegration/Sefaz/` orquestra cert do tenant (`CertificadoTenantResolver` + AES-GCM), assinatura XMLDSig C14N (`XmlSignerC14N`, SHA-1 conforme SEFAZ), SOAP/HTTPS mTLS (`SefazSoapClient` com Polly retry), catálogo de URLs (5 UFs prioritárias + SVRS + SVAN), e contingência (`ContingenciaPolicy`). Modelos POCO em `Acme.Sistemas.Domain/Entities/Fiscal/Xml/`. Sem dependência de lib externa de NF-e. Stub legado (`StubNFeSefazClient`) fica disponível como fallback dev via flag `Fiscal:UseStub=true` no `appsettings`

### Ponto Oficial 671 (`Acme.Sistemas.Services/V1/Rh/Oficial671/` + endpoints `/api/v1/rh/ponto/671/*`)

Conformidade do W4 com a **Portaria MTP 671/2021** — REP-C (cloud). Quando
`Empresa.UsaRepOficial=true`, toda batida do W2/Mobile chama o subfluxo 671:

- **NSR atômico** por `(tenant, empresa)`: `NumeradorNsr` reusa o idiom
  `INSERT … ON DUPLICATE KEY UPDATE LAST_INSERT_ID(col+1)` do `NumeradorNFe`.
  Pulos proibidos pela Portaria; auditados em 24h por `JobAuditarGapsNsrWorker`.
- **Comprovante anexo II** (`payload texto pipe-separated` + assinatura RSA-SHA-256
  ICP-Brasil) via `EmitirComprovante671` (`GeradorComprovantePontoTexto` →
  `AssinadorComprovante671` → `IComprovantePontoRepository`). PDF QuestPDF
  determinístico para 1ª via + 2ª via.
- **AFD** layout texto fixo 003 (`LayoutAfd003Writer` — tipos 1, 2, 3, 5, 9 cobertos
  no MVP; 4 e 6 zerados em PR follow-up). Hash SHA-256 do conteúdo no trailer.
- **AEJ** JSON v1 (`GeradorAejV1`) + JWS detached RFC 7515 (`AssinadorAej`).
- **Configuração REP** por empresa (`ConfiguracaoRep`): tipo (P/C), CNPJ/CEI/CNO,
  endereço, certificado vinculado, responsável legal. Auto-diagnóstico em
  `GET /671/validar/{empresaId}` checa cert + CNPJ no subject.
- **Endpoints**: `/671/configuracao`, `/671/validar`, `/671/comprovantes/{m}.pdf`,
  `/671/afd/exportar` + `/download`, `/671/aej/exportar` + `/download?formato={json|jws}`.
- **Permissões**: recurso `rh-ponto-oficial` + ações `configurar-rep`, `exportar-afd`,
  `exportar-aej`, `emitir-comprovante-2via`.
- **Frontend**: 3 telas em `site/atena-web/src/app/features/rh/ponto/oficial-671/`
  (`configuracao-rep`, `auto-diagnostico`, `exportar-afd-aej`).
- **Docs operacionais**: `documentacao/rh/ponto-oficial-671.md`.

### Mobile MAUI (`src/Mobile/`)

App nativo multi-plataforma (Android, iOS, Mac Catalyst, Windows) — alvo `net10.0-*`:

```
src/Mobile/
├── Acme.Sistemas.Atena.Mobile/         ← MAUI app (DI, Shell, ViewModels, Pages, Platforms/*)
├── Acme.Sistemas.Atena.Mobile.Shared/  ← DTOs + Helpers puros (net10.0, testáveis)
test/Mobile/
└── Acme.Sistemas.Atena.Mobile.Tests/   ← xUnit + FluentAssertions + Moq
```

- **MVVM** com `CommunityToolkit.Mvvm` (ObservableObject, [RelayCommand], [ObservableProperty])
- **HTTP** via `Refit` (`IAtenaApi`) com `AuthDelegatingHandler` (Bearer + refresh em 401) + Polly retry
- **Token store**: `SecureTokenStore` envolve `Microsoft.Maui.Storage.SecureStorage`
  (Keystore Android / Keychain iOS / PasswordVault Windows)
- **Offline queue**: `SqliteOfflineQueue` (sqlite-net-pcl) — enfileira batidas sem rede e sincroniza em `App.OnResume`/connectivity change
- **Bater ponto**: hash SHA-256 calculado no app (`HashHelpers.CalcularHashBatida`) e validado no servidor (`BaterPontoMobileCommandHandler`); foto via `MediaPicker`, GPS via `Geolocation`, biometria via stub (integração nativa em PR follow-up)
- **JWT mobile**: refresh token de **90 dias** (`Jwt:RefreshTokenDaysMobile`) vs 7 do web — `IJwtTokenService.IssueMobile()`
- **Endpoints backend**:
  - `POST /api/v1/autenticacao/login-mobile` (variante refresh longo)
  - `POST /api/v1/rh/ponto/bater-mobile` (multipart com foto + provaBiometriaLocal + hashBatida + timestampLocal ±5min + GPS)
  - `POST /api/v1/mobile/dispositivos/registrar` (idempotente por device_id, UNIQUE `tenant_id+usuario_id+device_id`)
  - `POST /api/v1/mobile/dispositivos/{deviceId}/desregistrar`
  - `GET /api/v1/mobile/configuracao` (versão mínima, banners, branding)
  - `GET /api/v1/admin/mobile/dispositivos` + `POST /api/v1/admin/mobile/dispositivos/{id}/revogar` (admin)
- **Push notifications**: `INotificacaoPushService` registrado como `StubNotificacaoPushService` (loga); `AprovarAjusteCommandHandler` publica para tópico `funcionario:{id}` quando ajuste é aprovado. Integração real Firebase Admin SDK / APNs HTTP/2 fica em PR `rh-mobile-push-fcm`/`rh-mobile-push-apns`.
- **CI/CD**: workflows em `.github/workflows/mobile-{android,ios,windows}.yml` com publicação opcional para Play Console (internal track) e TestFlight via `workflow_dispatch`
- **Docs operacionais**: `documentacao/rh/mobile/{setup-dev-windows,setup-dev-mac,distribuicao-android,distribuicao-ios,troubleshooting-usuario}.md`

### Frontend (`site/atena-web/`)

Angular 17 standalone com signals:

```
src/app/
├── core/
│   ├── auth/        → AuthStore (signals + JWT + refresh), guard, login, types
│   ├── branding/    → TenantBrandingService (CSS custom properties por tenant)
│   ├── permissions/ → permissaoGuard, *temPermissao directive
│   ├── http/        → authInterceptor, errorInterceptor
│   └── notifications/ → NotificacaoService (polling) + bell component
├── shared/
│   ├── data-table/  → Paginação server-side, debounce, ordenação por coluna
│   └── crud/        → CrudService, CrudListComponent, CrudFormComponent (genéricos)
├── layout/          → Default layout responsivo + Dashboard
└── features/        → financeiro, cadastros, estoque, compras, vendas, fiscal, relatorios, configuracao
```

### Domain Areas

| Área | Entidades principais |
|------|---------------------|
| Multi-tenancy | Tenant, TenantLimite |
| Segurança | Usuario, Role, Permission, RolePermission, UserRole, ApiKey, RefreshToken, TokenBlacklist |
| Financeiro | Despesa, Receita, FluxoDeCaixa, Divida, Pagamento, ContaPagar, ContaReceber, ConciliacaoBancaria, PlanoDeContas, CentroDeCusto |
| Cadastros | Empresa, Cliente, Fornecedor, Funcionario, Produto |
| Estoque | Estoque, EstoqueProduto, EntradaProdutoEstoque, SaidaProdutoEstoque, Inventario (FIFO) |
| Compras | SolicitacaoCompra, PedidoCompra, PedidoCompraItem, RecebimentoCompra |
| Vendas | Orcamento, PedidoVenda, PedidoVendaItem, Faturamento, DevolucaoVenda, ComissaoVendedor |
| Fiscal | ConfiguracaoFiscal, NFe, NFeItem, NFeEvento |
| **RH** (rh-fundacao W1) | **Jornada, Cargo, Lotacao, Departamento, EscalaFuncionario, HistoricoSalario, BeneficioCatalogo, BeneficioFuncionario, Dependente, Cbo (referência nacional). `Funcionario` estendido com cargo_id/lotacao_id/departamento_id, PIS/CTPS/RG, endereço JSON, conta bancária JSON. Validadores em `Core/Helper`: `PisHelper`, `CtpsHelper`, `ContaBancariaHelper`. Endpoints em `/api/v1/rh/*` (rh-funcionario, rh-cargo, rh-lotacao, rh-departamento, rh-beneficio, rh-jornada, rh-dependente). Frontend em `features/rh/` (wizard 4 passos + ficha completa em abas). Detalhes em `documentacao/rh/funcionario-modelo.md`.** |
| Auditoria | AuditLog, ApiRequestAudit |

## Database

- **MySQL** via Pomelo.EntityFrameworkCore.MySql
- Connection strings em `appsettings.json`
- EF Core migrations em `Acme.Sistemas.Infrastructure`
- Todo `Repository` herda `BaseRepository` que aplica filtro `WHERE tenant_id = @tenantId` automático

### Seed do super-admin (root)

A migration `V*_SeedRootAdmin.cs` em `src/Data/Acme.Sistemas.Infrastructure/Databases/Migrations/`
cria o tenant raiz, a role `Root` (com **todas** as permissões — incluindo `tenant:criar`, que é
filtrada para roles `Administrador` de tenants comuns em `CriarTenantCommandHandler`) e o usuário
super-admin com `Status=Ativo` e e-mail já confirmado.

**Convenção de versionamento:** o arquivo é **gitignored** (`.gitignore` linha `V*_SeedRootAdmin.cs`)
porque contém o e-mail e a senha do super-admin daquele ambiente. Cada dev / ambiente / cluster
mantém localmente a sua própria versão. Em CI a migration simplesmente não existe → o root não é
seedado, o que é intencional (ambientes de teste não devem ter credenciais hardcoded).

Para criar a sua: copiar o template `V20260512001_SeedRootAdmin.cs` (se ainda existir local)
ou pedir a um colega; trocar `RootEmail`, `RootSenha`, `AcmeCnpj`, `AcmeRazao` e `Version`
(timestamp único, formato `Vyyyymmddxxx`); rodar `dotnet build` + restart da API.

A migration roda dentro do `MigrationRunner` no boot, antes do host subir os hosted services.

### Seeds estáticos brasileiros e provisionamento de tenant

Migrations `V20260514xxx_*` semeiam catálogos de referência **nacionais** (não tenant-scoped):
`ufs` (27), `cfops` (subset curado), `csts_*` (ICMS/PIS/COFINS/IPI) e `codigos_servico_lc116`
(subset). Consultáveis via `GET /api/v1/cadastros/ufs` e `GET /api/v1/fiscal/{cfops|csts/{tipo}|codigos-servico}`.
Datasets volumosos (CFOP completo, NCM, Municípios) são opt-in/drop-in — ver `documentacao/seeds/README.md`.

**Provisionamento de tenant:** `POST /api/v1/admin/seed-tenant` (permissão `admin:seed-tenant`,
exclusiva do `Root`) cria tenant + admin + 5 roles + empresa + plano de contas + centros de custo
+ cliente/fornecedor/produto demo + config fiscal placeholder, **idempotente por CNPJ**. Rotas
`/api/v1/admin/*` passam pelo `AdminIpAllowlistMiddleware` (config `Admin:AllowedIps`, CIDRs;
loopback sempre liberado; lista vazia = sem restrição).

**Bootstrap dev:** em Development, com `Seed:AutoBootstrap=true` (default em
`appsettings.Development.json`) e banco sem tenant, o `DevTenantBootstrapHostedService` cria
`demo@atena.test` no boot e loga a senha no console. Nunca roda em Production (proteção dupla).
Detalhes em `documentacao/onboarding-tenant.md`.

## API Documentation

Swagger UI em `/swagger` (Development). Logs estruturados via NLog.

## Convenções de Testes

Todo método `[Fact]`/`[Theory]` (incluindo `[Fact(Skip = "...")]`) nos projetos `Acme.Sistemas.Services.UnitTest` e `Acme.Sistemas.IntegrationTest` declara três attributes:

```csharp
[Trait("Solucao", "Services")]                           // camada arquitetural — vocab fechado
[Trait("Acao", "CriarDespesa")]                          // unidade-em-teste — Command/Query/Helper
[Fact(DisplayName = "Dado X, quando Y, então Z")]        // frase Given-When-Then em PT-BR
```

**Solucao** ∈ `Api`, `Services`, `Core`, `Domain`, `Repository`, `Infrastructure`, `ExternalIntegration`, `Test`.

**Acao**: nome do Command/Query (`Login`, `CriarDespesa`), nome da classe (`AuditBehavior`, `JwtTokenService`), ou `Convencoes` para meta-tests.

Filtros úteis: `dotnet test --filter "Trait=Solucao=Services"` ou `dotnet test --filter "Trait=Acao=CriarDespesa"`.

Enforcement: `ConvencoesBlueprintTests.TodoTeste_TemDisplayNameESolucaoEAcao` reprova qualquer regressão. Detalhes em `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md`.
