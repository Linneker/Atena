# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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
| Auditoria | AuditLog, ApiRequestAudit |

## Database

- **MySQL** via Pomelo.EntityFrameworkCore.MySql
- Connection strings em `appsettings.json`
- EF Core migrations em `Acme.Sistemas.Infrastructure`
- Todo `Repository` herda `BaseRepository` que aplica filtro `WHERE tenant_id = @tenantId` automático

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
