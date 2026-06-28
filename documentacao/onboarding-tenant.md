# Onboarding de Tenant — Atena ERP

Este guia descreve como um ambiente Atena recém-instalado ganha **dados para rodar**:
catálogos brasileiros estáticos (semeados por migration) e tenants provisionados sob demanda.

## 1. Catálogos estáticos brasileiros (automático)

Aplicados por migration no boot (`MigrationRunner`), sem ação manual:

| Catálogo | Tabela | Conteúdo | Endpoint de consulta |
|----------|--------|----------|----------------------|
| UFs | `ufs` | 27 unidades federativas (IBGE) | `GET /api/v1/cadastros/ufs` |
| CFOPs | `cfops` | subset curado (~33) — ver nota | `GET /api/v1/fiscal/cfops?categoria=Entrada\|Saida` |
| CSTs | `csts_icms`, `csts_pis`, `csts_cofins`, `csts_ipi` | listas oficiais | `GET /api/v1/fiscal/csts/{icms\|pis\|cofins\|ipi}` |
| LC 116/03 | `codigos_servico_lc116` | subset curado (~74) | `GET /api/v1/fiscal/codigos-servico` |

> **Datasets volumosos** (CFOP completo ~700, NCM ~10k, Municípios ~5570) são opt-in e
> dependem de arquivo oficial externo — ver `documentacao/seeds/README.md`. Não são
> necessários para a operação básica nem para os fluxos de demonstração.

Os endpoints exigem apenas autenticação (qualquer usuário logado).

## 2. Provisionamento de tenant (endpoint admin)

```
POST /api/v1/admin/seed-tenant
Authorization: Bearer <token de usuário com permissão admin:seed-tenant>
Content-Type: application/json

{ "cnpj": "00000000000191", "razaoSocial": "Empresa Demo", "adminEmail": "admin@demo.test" }
```

Cria, em um único fluxo:
- `tenant` (plano FREE) + `tenant_limite`
- `usuario` admin (senha aleatória de 16 caracteres, **retornada uma única vez** na resposta) — já nasce **Ativo e com e-mail confirmado** (login imediato)
- 5 roles padrão: **Administrador** (tudo), **Financeiro**, **Operador**, **Fiscal**, **Visualizador**
- `empresa` demo
- `plano_de_contas` básico (5 grupos + 12 filhas)
- `centros_de_custo` (Administrativo, Comercial, Operacional)
- `cliente`, `fornecedor` e `produto` demo (para fluxos E2E)
- `configuracao_fiscal` placeholder (Homologação, sem certificado)

**Resposta:**
```json
{ "tenantId": "...", "adminUserId": "...", "senhaInicial": "Ax7...", "ehNovo": true }
```

### Idempotência
A chave é o **CNPJ**. Uma segunda chamada com o mesmo CNPJ retorna `ehNovo: false` e **não**
re-exibe a senha nem duplica entidades.

### Segurança
- Permissão `admin:seed-tenant` (apenas a role `Root`/super-admin a possui por padrão).
- Allowlist de IPs (`Admin:AllowedIps`, CIDRs) — IP fora da lista recebe **403** antes mesmo da autenticação. Loopback é sempre permitido; lista vazia = sem restrição.
- A resposta carrega `Cache-Control: no-store` (a senha não deve ser cacheada).

## 3. Bootstrap automático em Development

Em ambiente **Development**, com `Seed:AutoBootstrap=true` (default em
`appsettings.Development.json`) e **nenhum tenant** no banco, a API cria automaticamente no
primeiro boot o tenant:

```
Login: demo@atena.test
Senha: <logada no console com nível Warning>
```

A senha aparece no log do boot (`DevTenantBootstrap: tenant demo criado. Login: ... / Senha: ...`).
Em **Production** o bootstrap nunca roda (proteção dupla: registro condicional no DI +
verificação de ambiente no hosted service).

## 4. Super-admin (Root)

O usuário super-admin é semeado pela migration **gitignored** `V*_SeedRootAdmin.cs`
(por ambiente — contém credenciais). A role `Root` recebe **todas** as permissões, inclusive
`admin:seed-tenant` e `tenant:criar`. Ver seção "Seed do super-admin" no `CLAUDE.md`.
