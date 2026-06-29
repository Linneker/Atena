# Frontend Angular

## Propósito

SPA web em **Angular 17 standalone com signals** que consome a API REST. Layout
responsivo, branding por tenant, RBAC client-side via diretiva + guard, CRUD
genérico para reduzir boilerplate.

## Stack

- Angular 17 standalone components
- Signals para state local
- Lazy loading de features via `loadComponent`
- HttpClient + interceptors funcionais
- CSS custom properties para branding por tenant
- TypeScript strict

Path raiz: `site/atena-web/`. Aliases TS: `@env/environment` →
`src/environments/environment.ts`.

## Core

`site/atena-web/src/app/core/`:

| Pasta | Responsabilidade |
|-------|------------------|
| `auth/` | `AuthStore` (signals + JWT + refresh), `authGuard`, login, `auth.types.ts` |
| `branding/` | `TenantBrandingService` — aplica CSS custom properties (cor primária, logo) por tenant |
| `permissions/` | `permissaoGuard` (route-level), `*temPermissao` directive (template-level) |
| `http/` | `authInterceptor` (Bearer), `errorInterceptor` (401 → refresh ou logout) |
| `notifications/` | `NotificacaoService` (polling) + bell component no header |

## AuthStore — signals + JWT

```typescript
class AuthStore {
  user = signal<UserClaims | null>(null);
  permissions = signal<string[]>([]);
  isAuthenticated = computed(() => !!this.user());
  login(email, senha): Observable<void> { ... }
  logout(): Observable<void> { ... }
  refresh(): Observable<void> { ... }
  hasPermission(p: string): boolean { ... }
}
```

JWT decodificado para popular `user` + `permissions`. Refresh transparente em
401 via interceptor.

## Shared

`site/atena-web/src/app/shared/`:

| Pasta | Componente / Service |
|-------|----------------------|
| `data-table/` | `DataTableComponent` com paginação server-side, debounce 300ms, ordenação por coluna, slot para template de célula |
| `crud/` | `CrudService<T>` genérico (CRUD REST), `CrudListComponent`, `CrudFormComponent` — reduz boilerplate para CRUDs simples |

## Layout

`site/atena-web/src/app/layout/`:
- `default-layout/` — sidebar + header + outlet (responsivo: drawer no mobile)
- `dashboard/` — KPIs + atalhos por permissão do usuário

## Features

Cada subpasta de `features/` é uma área isolada com sub-rotas:

| Feature | Sub-rotas / componentes notáveis |
|---------|----------------------------------|
| `auth/` | login, primeiro-acesso, reset-senha |
| `cadastros/` | empresas, clientes, fornecedores, funcionários, produtos (com wizard de funcionário em 4 passos) |
| `financeiro/` | despesas, receitas, contas-pagar, contas-receber, fluxo-caixa, conciliação, plano-contas |
| `estoque/` | estoques, saldos, entradas, saídas, inventários |
| `compras/` | solicitações, pedidos, recebimentos |
| `vendas/` | orçamentos, pedidos, faturamentos, devoluções, comissões |
| `fiscal/` | NF-es, certificado, configuração |
| `relatorios/` | TODO — geração de relatórios |
| `configuracao/` | empresa, usuários, roles, permissões |
| `rh/` | funcionários (wizard + abas), cargos, lotações, departamentos, jornadas, benefícios, **ponto** (7 telas) |

## RH Ponto (W2 + W4 frontend)

`site/atena-web/src/app/features/rh/ponto/`:
- `meu-ponto/` — funcionário bate ponto
- `espelho/` — calendário mensal
- `aprovacoes/` — gestor aprova ajustes
- `banco-horas/` — saldos + políticas
- `fechamento/` — RH fecha competência
- `oficial-671/` — config REP + auto-diagnóstico + exportar AFD/AEJ

## Comandos

```powershell
cd site/atena-web
npm install
npm start            # dev server localhost:4200
npm run build:prod   # build de produção
npm test             # Karma + Jasmine
```

## Decisões

- **Standalone components**: sem NgModules. Cada componente declara
  `imports: [CommonModule, FormsModule, ...]`.
- **Signals** em vez de RxJS para state local — RxJS continua para
  HTTP/streaming.
- **Lazy loading via `loadComponent`** (não loadChildren) — direto ao componente
  da rota.
- **CrudService genérico**: aceita config `{ endpoint, displayColumns, formSchema }`
  — telas simples (departamento, cargo, etc.) são quase declarativas.
- **Branding**: CSS custom properties no `<body>` aplicadas via
  `TenantBrandingService.aplicar(branding)` no boot pós-login.

## Arquivos para consultar

- `site/atena-web/src/app/core/auth/auth.store.ts`
- `site/atena-web/src/app/core/branding/tenant-branding.service.ts`
- `site/atena-web/src/app/core/permissions/permissao.guard.ts` + `tem-permissao.directive.ts`
- `site/atena-web/src/app/core/http/auth.interceptor.ts` + `error.interceptor.ts`
- `site/atena-web/src/app/shared/data-table/`
- `site/atena-web/src/app/shared/crud/`
- `site/atena-web/src/app/features/` (uma pasta por área)
- `site/atena-web/angular.json`, `package.json`

## Follow-ups conhecidos

- i18n (atualmente só PT-BR).
- Dark mode.
- Migração para Angular 18+ control flow (`@for`/`@if` já usados no W3 671;
  resto ainda usa `*ngFor`/`*ngIf`).
- Storybook para `data-table` e `crud` genéricos.
