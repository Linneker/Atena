# Plataforma & Convenções

## Propósito

Camada transversal: como tenants, usuários, permissões, autenticação e o blueprint
arquitetural funcionam. Todo desenvolvimento novo passa por aqui — `tenant_id` em
todas as tabelas, permissões em todos os endpoints, vertical CQRS em todo command/query.

## Multi-tenancy

- Toda tabela tem `tenant_id CHAR(36) NOT NULL`.
- `ITenantContext` (scoped, namespace `Acme.Sistemas.Domain.Interfaces.Repository`) é
  injetado pelo `TenantMiddleware` extraindo o claim `tid` do JWT.
- `BaseRepository<T>` aplica `WHERE tenant_id = @tenantId` automático em todas as
  queries — handlers nunca precisam pensar nisso.
- `IMutableTenantContext` existe para cenários administrativos (impersonação,
  bootstrap, jobs).

## Autenticação JWT + Refresh + Blacklist

- Login devolve **access token** (curto, com claims de permissão) + **refresh token**
  (longo, na tabela `refresh_tokens`).
- Logout grava refresh token na `token_blacklist` — `JwtBlacklistEvents` no
  `JwtBearer` rejeita tokens revogados.
- `JwtTokenService.Issue()` para web (refresh 7 dias) e `IssueMobile()` para mobile
  (refresh 90 dias, config `Jwt:RefreshTokenDaysMobile`).
- Algoritmo: `HS256` com `Jwt:SigningKey` (32+ bytes recomendado).

## RBAC

- 4 tabelas: `roles`, `permissions`, `role_permissions`, `user_roles`.
- Permissões expressas como `"recurso:acao"` em `Acme.Sistemas.Core.Const.Permissions`.
  - `Recursos`: 50+ constantes (Empresa, Despesa, RhPonto, RhPontoOficial, etc.).
  - `Acoes`: Ler, Criar, Editar, Excluir, Aprovar, Faturar, Exportar, +
    ações específicas (BaterPonto, ConfigurarRep, ExportarAfd, etc.).
- `Permissions.All()` gera produto cartesiano para seeding.
- Endpoints declaram via extension `.RequirePermissao(Permissions.Of(...))` em
  `Acme.Sistemas.Atena.Api.Config.Security`.
- `PermissaoAuthorizationHandler` (Singleton) valida claim `permissions` no JWT.

## Blueprint Acme — CQRS Mediator próprio

Em `Acme.Sistemas.Core.Mediators`:
- `IRequest<TResponse>` + `IRequestHandler<TRequest, TResponse>`
- 4 behaviors transversais em ordem: **Validation → CacheLookup → Audit → Log → Behavior específico → Handler**
- `services.AddAcmeMediator(assembly)` descobre handlers e behaviors via reflexão.

## Vertical Pattern — 5 arquivos por Command/Query

```
V1/<Funcionalidade>/Command/<Acao>/
├── <Acao>Command.cs               record + IRequest<ResponseDefault<...>>
├── <Acao>CommandHandler.cs        IRequestHandler<...>
├── <Acao>CommandBehavior.cs       IPipelineBehavior<...> (cache invalid., regras extras)
├── <Acao>CommandResult.cs         record imutável
└── <Acao>CommandValidation.cs     AbstractValidator FluentValidation
```

Queries idem, com `Query`/`QueryHandler`/`QueryBehavior`/`QueryResult`/`QueryValidation`.

## Endpoint Pattern — 4 arquivos por rota

```
Api/Endpoints/V1/{Recurso}/{Verbo}{Recurso}/
├── {Verbo}{Recurso}Endpoint.cs    classe IEndpoint que registra a rota
├── {Verbo}{Recurso}Request.cs     DTO de entrada
├── {Verbo}{Recurso}Response.cs    DTO de saída
└── {Verbo}{Recurso}Map.cs         Request→Command, Result→Response
```

Descoberta automática via `EndpointRegistrationExtensions` (reflexão sobre
`IEndpoint`). Validada em runtime por `EndpointConventionTests` no projeto
Integration.

## Tenant root e provisionamento

- Migration `V*_SeedRootAdmin.cs` **gitignored** — cria tenant raiz + role
  `Root` (todas permissões incluindo `tenant:criar`) + super-admin.
- `tenant:criar` é exclusiva do Root, filtrada de roles `Administrador` de
  tenants comuns em `CriarTenantCommandHandler`.
- `POST /api/v1/admin/seed-tenant` (permissão `admin:seed-tenant`) cria tenant
  completo, idempotente por CNPJ.
- `/api/v1/admin/*` passa por `AdminIpAllowlistMiddleware` (config `Admin:AllowedIps`,
  CIDR; loopback sempre liberado).
- Dev: `Seed:AutoBootstrap=true` (default em Development) +
  `DevTenantBootstrapHostedService` cria `demo@atena.test` no boot.

## Endpoints REST chave

| Método | Rota | Permissão | Descrição |
|--------|------|-----------|-----------|
| POST | `/api/v1/autenticacao/login` | público | Login web (refresh 7d) |
| POST | `/api/v1/autenticacao/login-mobile` | público | Login mobile (refresh 90d) |
| POST | `/api/v1/autenticacao/refresh` | público | Renova access via refresh |
| POST | `/api/v1/autenticacao/logout` | autenticado | Joga refresh na blacklist |
| POST | `/api/v1/admin/seed-tenant` | `admin:seed-tenant` (Root) | Provisiona tenant + admin |
| POST | `/api/v1/tenants/registrar` | autenticado | Cria tenant (requer auth) |

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Core/Mediators/` — Mediator próprio + behaviors
- `src/Service/Acme.Sistemas.Core/Const/Permissions.cs` — recursos + ações
- `src/Service/Acme.Sistemas.Core/Security/JwtTokenService.cs` — Issue / IssueMobile
- `src/Api/Acme.Sistemas.Atena.Api/Config/Security/` — `PermissaoAuthorization`,
  `JwtBlacklistEvents`, `HttpTenantContextAccessor`, `TenantClaims`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/EndpointRegistrationExtensions.cs`
- `src/Service/Acme.Sistemas.Services/V1/Admin/Command/SeedTenant/`
- `documentacao/blueprint.yml` (norma técnica)
- `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md` (guia)
- `documentacao/templates/` (templates de Behavior, Result, Validation, etc.)
- `documentacao/onboarding-tenant.md`

## Follow-ups conhecidos

- SSO via OIDC externo (Google/Microsoft) — não há ainda.
- Permissões por escopo de empresa (multi-empresa por tenant).
