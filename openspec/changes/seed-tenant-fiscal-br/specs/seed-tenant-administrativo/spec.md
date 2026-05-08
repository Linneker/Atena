## ADDED Requirements

### Requirement: Catálogo de Dados Estáticos Brasileiros
O sistema SHALL manter catálogos pré-carregados via migration: UFs (27), CFOPs (~700), CSTs ICMS/PIS/COFINS/IPI (~40), Códigos de Serviço LC 116/03 (123). NCMs (~10000) e Municípios IBGE (~5570) SHALL ser opt-in via feature flag ou endpoint admin.

#### Scenario: Migration popula tabelas obrigatórias
- **WHEN** o sistema é deployed em DB limpo
- **THEN** após `dotnet ef database update`, as tabelas `ufs`, `cfops`, `csts_*`, `codigos_servico_lc116` ficam populadas
- **THEN** `SELECT COUNT(*) FROM ufs` retorna 27
- **THEN** `SELECT COUNT(*) FROM cfops` retorna pelo menos 700

#### Scenario: NCMs opt-in via endpoint admin
- **WHEN** admin chama `POST /api/v1/admin/seed-fiscal-br` com `{ "incluir": ["ncm"] }`
- **THEN** o sistema carrega ~10000 NCMs em < 30s
- **THEN** chamada repetida é idempotente (não duplica)

### Requirement: Endpoint Admin de Seed-Tenant
O sistema SHALL expor `POST /api/v1/admin/seed-tenant` que provisiona tenant idempotentemente, criando tenant + usuário admin + empresa demo + plano de contas + centros de custo + cliente/fornecedor/produto demo + configurações fiscais placeholder em transação única.

#### Scenario: Criação de tenant novo
- **WHEN** super-admin chama `POST /api/v1/admin/seed-tenant` com `{ cnpj: "00000000000191", razaoSocial: "Empresa Demo", adminEmail: "admin@demo.test" }`
- **THEN** o sistema cria tenant + admin user com senha aleatória + entidades de bootstrap em transação
- **THEN** retorna `{ tenantId, adminUserId, senhaInicial: "...", ehNovo: true }`
- **THEN** response tem header `Cache-Control: no-store`
- **THEN** entrada de auditoria é gravada

#### Scenario: Idempotência por CNPJ
- **WHEN** super-admin chama `seed-tenant` 2x com mesmo CNPJ
- **THEN** primeira chamada cria todas as entidades
- **THEN** segunda chamada retorna `{ tenantId, adminUserId, ehNovo: false }` (sem `senhaInicial`)
- **THEN** nenhuma entidade duplicada no DB

#### Scenario: Acesso negado a usuário comum
- **WHEN** usuário sem role `SuperAdmin` chama `seed-tenant`
- **THEN** sistema retorna 403 Forbidden
- **THEN** auditoria registra tentativa

#### Scenario: IP fora da allowlist
- **WHEN** chamada vem de IP fora de `Admin.AllowedIps` configurado
- **THEN** sistema retorna 403 antes de validar autenticação
- **THEN** logs registram IP bloqueado

### Requirement: Bootstrap Automático em Dev
O sistema SHALL prover hosted service `DevTenantBootstrapHostedService` que, quando `Seed.AutoBootstrap=true` E ambiente é Development E não há tenant no DB, cria tenant `demo@atena.test` automaticamente na primeira subida.

#### Scenario: Primeira subida em dev
- **WHEN** desenvolvedor sobe `dotnet run` em ambiente Development com DB recém-criado
- **THEN** o hosted service cria tenant `demo@atena.test`
- **THEN** senha admin é logada no console (apenas em Dev)
- **THEN** desenvolvedor pode fazer login imediatamente

#### Scenario: Tentativa em Production
- **WHEN** ambiente é Production
- **THEN** o hosted service não roda mesmo com `AutoBootstrap=true` (proteção dupla)

### Requirement: Roles Padrão
O sistema SHALL semear 5 roles padrão por tenant durante seed-tenant: `Admin` (todas permissões), `Financeiro` (recursos financeiros + relatórios), `Operador` (cadastros, vendas, compras, estoque - read+write), `Fiscal` (NFe, NFSe, configuração fiscal, auditoria), `Visualizador` (todos recursos read-only).

#### Scenario: Tenant novo recebe roles padrão
- **WHEN** seed-tenant cria tenant
- **THEN** as 5 roles padrão são criadas com nomes traduzidos
- **THEN** permissões granulares estão atribuídas conforme matriz documentada

### Requirement: Plano de Contas Inicial
O sistema SHALL semear plano de contas básico com 5 grupos contábeis (Ativo, Passivo, Patrimônio, Receitas, Despesas) e ~30 contas filhas.

#### Scenario: Plano de contas customizável
- **WHEN** seed-tenant cria plano de contas
- **THEN** estrutura básica funciona para fluxos financeiros default
- **THEN** admin do tenant pode adicionar/editar/remover contas via UI

### Requirement: Catálogo IBGE Acessível via API
O sistema SHALL expor endpoints públicos (autenticados) para consulta dos catálogos brasileiros, com paginação e filtros básicos.

#### Scenario: Lookup de CFOPs por categoria
- **WHEN** usuário chama `GET /api/v1/fiscal/cfops?categoria=Saidas`
- **THEN** retorna lista paginada de CFOPs categoria Saída (5xxx)
- **THEN** cache via `IDistributedCache` por 1 hora (dados estáticos)

#### Scenario: Lookup de UFs
- **WHEN** usuário chama `GET /api/v1/cadastros/ufs`
- **THEN** retorna 27 UFs ordenadas por sigla
- **THEN** resposta cacheada agressivamente
