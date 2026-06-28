# Matriz role × permissões — Módulo RH (rh-fundacao, W1)

> Atualizado em 2026-06-28 pela onda `rh-fundacao`.
> Todas as permissões deste arquivo são auto-semeadas em `permissions` no boot
> via `PermissionsSeedHostedService`, que percorre `Permissions.All()`
> (cross-product de `Recursos` × `Acoes` em `Permissions.cs`).

## Recursos RH novos

| Recurso (chave) | Cobre |
|-----------------|-------|
| `rh` | Acesso ao módulo RH (root, controla menu lateral) |
| `rh-funcionario` | CRUD de funcionários e ficha completa |
| `rh-jornada` | CRUD de jornadas de trabalho |
| `rh-cargo` | CRUD de cargos (com CBO opcional) |
| `rh-lotacao` | CRUD de lotações (estabelecimentos) |
| `rh-departamento` | CRUD de departamentos |
| `rh-beneficio` | CRUD de catálogo de benefícios + atribuição a funcionários |
| `rh-dependente` | Dependentes para IRRF e salário-família |

## Ações que se aplicam a cada recurso RH

Toda combinação `rh-*:<acao>` está disponível para as ações padrão (`ler`, `criar`, `editar`, `excluir`).
Ação especial:

| Ação | Significado | Tipicamente combinada com |
|------|-------------|---------------------------|
| `gerir-equipe` | Gestor vê/edita apenas funcionários sob sua hierarquia (não todos do tenant) | `rh-funcionario:gerir-equipe` |

## Roles seedadas por `SeedTenantCommandHandler`

| Role | Inclui permissões RH? | Detalhes |
|------|----------------------|----------|
| **Root** (super-admin global) | Sim, todas — inclusive cross-tenant | `tenant:criar` e `admin:seed-tenant` são exclusivos. |
| **Administrador** (admin do tenant) | Sim, todas `rh-*:*` | Recebe todas as permissões `Grantable` (todas menos `tenant:criar` e `admin:seed-tenant`). |
| **RH** *(nova em W1)* | Sim, todas `rh-*:*` | Para a equipe de RH; sem acesso ao financeiro/vendas/fiscal/compras. |
| **Financeiro** | Não | Recursos financeiros apenas. |
| **Operador** | Não | Cadastros + vendas/compras (Ler/Criar/Editar). |
| **Fiscal** | Não | NF-e + config fiscal + auditoria. |
| **Visualizador** | `rh-*:ler` (somente leitura) | Filtra por ação `ler`, então recebe leitura RH também. |

## Defaults RH semeados em todo tenant novo

Após `SeedTenantCommandHandler` rodar, o tenant já tem:

| Entidade | Valor seedado |
|----------|---------------|
| `jornadas` | `"44h CLT"` — Fixa, 44h/semana, seg-sex 08:00-12:00 / 13:30-17:30 + sáb 08:00-12:00, tolerância 10 min |
| `cargos` | `"NAO-CLASS"` — "Não classificado" (placeholder para funcionários ainda sem cargo) |
| `departamentos` | `"NAO-CLASS"` — "Não classificado" |
| `lotacoes` | `"Sede"` — lotação default sem CNPJ próprio |

Cada seed é **idempotente** — só insere se ainda não existir um registro com o mesmo
código/nome. Re-rodar `SeedTenant` para um tenant existente não duplica defaults.

## Como adicionar uma permissão RH nova nas ondas seguintes

1. Adicionar a constante em `Recursos` ou `Acoes` em `src/Service/Acme.Sistemas.Core/Const/Permissions.cs`.
2. Atualizar o `HashSet<string> rhRecursos` em `SeedRolesAsync` no `SeedTenantCommandHandler`
   se a nova permissão deve fazer parte da role `RH`.
3. Boot da API roda `PermissionsSeedHostedService` que insere a nova permissão na tabela
   `permissions`. Tenants existentes recebem a permissão automaticamente; quem precisa
   herdar via role deve receber via UI de Roles ou via re-execução do seed.
