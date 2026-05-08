# Design — seed-tenant-fiscal-br

## Estratégia

Duas frentes complementares:

1. **Dados estáticos brasileiros** (UFs, CFOPs, NCMs, CSTs, LC 116, municípios) — via migration EF Core com seeds embedded.
2. **Provisionamento dinâmico de tenant** — via endpoint admin idempotente.

```
   ┌──────────────────────────────────────────────────────┐
   │  STARTUP                                             │
   │  ─ Migration aplica tabelas BR + seeds estáticos    │
   │  ─ PermissionsSeedHostedService aplica RBAC seeds   │
   │  ─ (Dev) AutoBootstrap cria tenant demo se vazio    │
   └─────────────────────┬────────────────────────────────┘
                         ▼
   ┌──────────────────────────────────────────────────────┐
   │  POST /api/v1/admin/seed-tenant                      │
   │  Body: { cnpj, razaoSocial, adminEmail }            │
   │  ─ super-admin role required                         │
   │  ─ idempotente (cnpj é a chave)                      │
   │  ─ retorna { tenantId, adminUserId, senhaInicial }  │
   └─────────────────────┬────────────────────────────────┘
                         ▼
   ┌──────────────────────────────────────────────────────┐
   │  Cria registros em transaction:                      │
   │  ─ tenant + tenant_limite                            │
   │  ─ usuario (admin) com senha aleatória               │
   │  ─ user_role (Admin)                                 │
   │  ─ empresa demo                                      │
   │  ─ plano_de_contas (estrutura básica)                │
   │  ─ centros_de_custo (3-5)                            │
   │  ─ cliente, fornecedor, produto demo                 │
   │  ─ configuracao_fiscal placeholder                   │
   └──────────────────────────────────────────────────────┘
```

## Decisões e tradeoffs

### Por que migration ao invés de scripts SQL?
- EF Core já é a ferramenta de migration do projeto (Pomelo MySQL).
- Migration C# permite lógica condicional (e.g., só carrega NCM se feature flag).
- Permite rollback e versionamento auditável.

### NCMs — opt-in ou padrão?
NCM tem ~10000 linhas; seed pesado. Decisão: **opt-in via feature flag** `Seed.LoadNcms=true`. Default false. Migration cria a tabela vazia; admin chama `POST /api/v1/admin/seed-fiscal-br?incluir=ncm` quando precisar.

### CFOPs — quais incluir?
Lista oficial Receita Federal (anexo do RICMS) tem ~700 CFOPs. Todos incluídos no seed (peso pequeno). Versionados por `seed_version` para upgrade futuro.

### Municípios IBGE — opt-in?
~5570 linhas. Default false; opt-in via flag. Quem precisar de validação de município no cadastro de cliente/fornecedor ativa.

### Idempotência do seed-tenant
Chave: `cnpj`. Procedimento:
1. `SELECT id FROM tenants WHERE cnpj = ?` → se existe, retorna o existente sem mudar nada.
2. Senão, transaction completa cria tudo.
3. Resposta sempre inclui `tenantId`; senha admin só retorna em criação nova.

### Geração de senha admin
- 16 caracteres aleatórios (hex).
- Hash com `BCrypt.Net-Next` (já usado).
- Senha plain retornada **uma vez** na resposta HTTP (não armazenada em log).
- Resposta marcada com `Cache-Control: no-store`.

### CNPJ demo
- Gerador determinístico: hash do nome do tenant → 14 dígitos com DV mod 11.
- Coluna `is_demo` marcada para tenants criados sem CNPJ real.
- Filtro em queries de produção: por padrão exclui demos (parametrizável).

### Roles padrão
| Role | Permissões |
|------|------------|
| Admin | TODAS |
| Financeiro | Recursos.Despesa, Receita, ContasPagar, ContasReceber, Pagamento, FluxoDeCaixa, Relatorios — Acoes.* |
| Operador | Vendas, Compras, Estoque, Cadastros — Acoes.Listar, Obter, Criar, Alterar |
| Fiscal | NFe, NFSe, ConfiguracaoFiscal, Auditoria — Acoes.* |
| Visualizador | TODOS recursos — Acoes.Listar, Obter (read-only) |

### Plano de contas básico
Estrutura mínima funcional:
- 1 — Ativo
  - 1.1 Ativo Circulante
    - 1.1.1 Caixa e equivalentes
    - 1.1.2 Contas a Receber
    - 1.1.3 Estoque
- 2 — Passivo
  - 2.1 Passivo Circulante
    - 2.1.1 Fornecedores
    - 2.1.2 Contas a Pagar
- 3 — Patrimônio
  - 3.1 Capital Social
- 4 — Receitas
  - 4.1 Vendas
  - 4.2 Serviços
- 5 — Despesas
  - 5.1 Operacionais
  - 5.2 Tributárias

Customizações ficam por conta do tenant via UI.

### Segurança do endpoint admin
- Permissão `Recursos.Admin, Acoes.SeedTenant` (nova).
- Role `SuperAdmin` (única role que tem essa permissão por padrão).
- Allowlist de IPs configurável (`Admin.AllowedIps`).
- Audit log entry obrigatório em cada chamada (já coberto pelo `AuditBehavior`).

## Test strategy

- **Unit**: idempotência do seed-tenant (rodar 2x → mesmo resultado, sem duplicatas).
- **Integration**: `SeedTenantEndpointTests` cria tenant real no DB de teste.
- **Migration**: `MigrationRunnerTests` valida que tabelas BR ficam preenchidas.
- **E2E reativados**: `FluxoVendaCompletaTests` e `IsolamentoCrossTenantTests` passam pós-seed.
