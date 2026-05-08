## Why

Hoje o `PermissionsSeedHostedService` semeia apenas roles/permissions. Quando um cliente novo entra no sistema:
1. Banco está vazio de dados de domínio brasileiro (UFs, CFOPs, NCMs, CSTs, códigos de serviço LC 116).
2. Não há tenant de demo nem comando administrativo para criar tenant idempotente.
3. Os testes E2E `Fluxo_Login_PedidoVenda_Faturamento_NFe_DeveCompletar` e `Fluxo_Compra_Recebimento_Estoque_ContaPagar_DeveCompletar` estão `[Skip]` exatamente porque "requer seed completo".

Sem essa base, **o ERP não tem onde rodar**: tela de produto pede NCM, tela de NFe pede CFOP, fiscal exige CSTs, NFS-e exige código LC 116. Manualmente cadastrar isso é inviável.

## What Changes

- **Tabelas auxiliares BR** (migration única, ~30k linhas no total):
  - `ufs` (27 entradas: sigla, nome, codigo_ibge)
  - `cfops` (~700 entradas, lista oficial Receita Federal)
  - `ncms` (~10000 entradas, NCM 8 dígitos com descrição) — opcional via seed flag
  - `csts_icms`, `csts_pis`, `csts_cofins`, `csts_ipi` (algumas dezenas cada)
  - `codigos_servico_lc116` (123 entradas — também usado pela change `nfse-abrasf-pluggavel`)
  - `municipios` (~5570 com codigo_ibge, nome, uf) — opcional
- **API admin de seed de tenant**: endpoint `POST /api/v1/admin/seed-tenant` que cria tenant idempotente com:
  - Tenant + admin user + senha aleatória (retornada uma vez)
  - Empresa demo (CNPJ válido fictício)
  - Plano de contas básico (5 grupos, 30 contas)
  - Centros de custo padrão (3-5)
  - Roles padrão (Admin, Operador, Fiscal, Financeiro)
  - Permissões granulares atribuídas às roles
  - Cliente, fornecedor e produto demo (para fluxos E2E)
  - Configuração fiscal placeholder (pendente de cert)
- **Endpoint admin de seed parcial** (`POST /api/v1/admin/seed-fiscal-br`): re-roda seeds das tabelas BR (idempotente).
- **Hosted service de bootstrap dev**: em `appsettings.Development.json`, flag `Seed.AutoBootstrap=true` cria tenant `demo@atena.test` na primeira subida.
- **Reativação dos testes E2E**: agora podem rodar contra ambiente seed-completo.

## Capabilities

### New Capabilities

- `seed-tenant-administrativo`: API admin para provisionar tenant novo idempotente, com dados-base brasileiros pré-carregados.

### Modified Capabilities

- `multi-tenancy`: estende com seed-tenant administrativo.

## Out of Scope

- Wizard de onboarding no frontend (decidido: SQL/API admin, não wizard).
- Importação de produtos/clientes via planilha Excel — outra change.
- Migração de outros ERPs — outra change.

## Risks

- **Migrations grandes**: NCMs (~10k linhas) podem demorar minutos. Mitigação: feature flag `Seed.LoadNcms=false` por default, opt-in.
- **CFOPs evoluem**: lista oficial muda anualmente. Mitigação: versionar seed (`seed_version`) e re-rodar via endpoint admin.
- **Senha admin retornada**: endpoint precisa autorização especial (super-admin role + IP allowlist).
- **CNPJ fictício**: usar gerador determinístico para tenants demo, marcar coluna `is_demo=true`.

## Success Criteria

- Migration roda em CI e em prod sem timeout (< 60s).
- Endpoint `POST /api/v1/admin/seed-tenant` cria tenant funcional em < 5s.
- Idempotência: chamada repetida com mesmo CNPJ retorna mesmo resultado sem duplicar.
- Testes `Fluxo_Login_PedidoVenda_Faturamento_NFe_DeveCompletar` e `Fluxo_Compra_Recebimento_Estoque_ContaPagar_DeveCompletar` reativados (sem `[Skip]`) e verdes em CI.
- Catálogo BR completo (UFs, CFOPs, CSTs, LC 116) consultável via API.
- Documentação clara em `documentacao/onboarding-tenant.md`.
