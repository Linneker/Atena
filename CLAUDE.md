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
# Bring up the full stack (API, MySQL, Redis, RabbitMQ, MinIO)
docker compose up -d

# Build API image only (from src/Api/Acme.Sistemas.Atena.Api/)
docker build -t atena-api .
```

### Kubernetes

Manifests live under `infra/k8s/v1/` (deployment, service, configmaps).

```powershell
kubectl apply -f infra/k8s/v1/
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

### Key Patterns

- **CQRS via Mediator próprio** (em `Acme.Sistemas.Core/Mediador/`): Commands em `V1/<Recurso>/Command/`, Queries em `V1/<Recurso>/Query/` — cada um com `Command|Query`, `Handler`, `Behavior`, `Result`, `Validation`
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
