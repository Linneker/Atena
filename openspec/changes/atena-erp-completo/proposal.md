## Why

O Atena iniciou como um sistema financeiro parcialmente scaffolded e precisa evoluir para um ERP SaaS multi-tenant completo com vendas, estoque, compras, emissão de NF-e e frontend unificado em Angular/CoreUI — substituindo os três frontends paralelos atuais e alinhando o código ao padrão arquitetural Acme (blueprint.yml).

## What Changes

- **Consolidação de frontend**: eliminar `cashflow/`, `cashflow2/` e o MVC site; construir todas as telas no CoreUI Angular (já existente em `site/coreui-free-angular-admin-template/`)
- **Migração arquitetural**: refatorar os 7 projetos `.NET` atuais (`acme.atena.*`) para o padrão Acme blueprint — Minimal API, CQRS com Command/Query/Event/Behavior por funcionalidade, SQL puro no Repository, infra separada
- **Multi-tenancy SaaS**: isolar todos os dados por tenant (empresa cliente), incluindo plano de assinatura, branding e configurações próprias
- **Módulo Financeiro completo**: implementar lógica de negócio real para Dívida, Pagamento e FluxoDeCaixa; adicionar Contas a Pagar, Contas a Receber, Conciliação Bancária e relatórios DRE/Balanço
- **Módulo de Cadastros completo**: implementar Fornecedor, Cliente, Funcionário, Centro de Custo e Plano de Contas
- **Módulo Estoque**: expor endpoints REST; implementar entrada/saída/saldo com rastreabilidade
- **Módulo Compras**: pedido de compra, recebimento, aprovação e baixa automática no estoque
- **Módulo Vendas**: pedido de venda, faturamento, devolução e integração com NF-e
- **Fiscal NF-e**: emissão de NF-e (modelo 55) via SEFAZ com certificado digital A1/A3, DANFE e XML; cancelamento e carta de correção
- **Segurança RBAC**: substituir o sistema de permissão atual por Roles/Permissions granulares com controle de acesso por tela e ação
- **Dashboard e Relatórios**: painel executivo com KPIs financeiros e operacionais; relatórios exportáveis (PDF/Excel)
- **Auditoria**: log funcional de todas as mutações com usuário, tenant, timestamp e dados anteriores/posteriores

## Capabilities

### New Capabilities

- `multi-tenancy`: Isolamento SaaS completo — cada empresa cliente (tenant) tem dados, configurações e branding independentes
- `modulo-financeiro`: Módulo financeiro completo — Despesa, Receita, Dívida, Pagamento, Fluxo de Caixa com fechamento, Contas a Pagar, Contas a Receber, Conciliação Bancária, DRE e Balanço Patrimonial
- `modulo-cadastros`: Cadastros mestres completos — Cliente, Fornecedor, Funcionário, Centro de Custo, Plano de Contas, Produto, Tipo de Produto e Tabela de Preços
- `modulo-estoque`: Gestão de estoque — saldo, entradas, saídas, transferências, inventário e relatório de movimentação
- `modulo-compras`: Compras completo — solicitação, pedido de compra, aprovação, recebimento e baixa automática no estoque
- `modulo-vendas`: Vendas completo — orçamento, pedido de venda, faturamento, devolução e integração com NF-e
- `fiscal-nfe`: Emissão fiscal — NF-e modelo 55 com SEFAZ, certificado digital A1/A3, DANFE, cancelamento e carta de correção
- `seguranca-rbac`: Segurança granular — Roles, Permissions, controle de acesso por recurso e ação, API Keys para integrações
- `dashboard-relatorios`: Dashboard executivo e relatórios — KPIs de vendas, financeiros e estoque; exportação PDF/Excel
- `auditoria`: Rastreabilidade completa — log imutável de criações, alterações e exclusões por usuário e tenant
- `frontend-coreui`: Frontend unificado — consolidar todos os módulos ERP em um único projeto Angular/CoreUI, eliminando os frontends paralelos
- `refatoracao-arquitetura`: Migração para padrão Acme — reestruturar projetos .NET conforme blueprint.yml (Minimal API, CQRS por funcionalidade, SQL puro, infra separada)

### Modified Capabilities

_(nenhuma — o projeto não possui specs existentes; todas as capacidades são novas)_

## Impact

**Backend (.NET)**
- Os 7 projetos `acme.atena.*` serão reestruturados nos projetos padrão Acme: `Acme.Sistemas.Atena.Api`, `Acme.Sistemas.Services`, `Acme.Sistemas.Core`, `Acme.Sistemas.Domain`, `Acme.Sistemas.Repository`, `Acme.Sistemas.Infrastructure`, `Acme.Sistemas.ExternalIntegration`
- Controllers MVC (`acme.atena.api`) migram para Minimal API com endpoints versionados (`/api/v1/...`)
- Toda lógica de aplicação migra para CQRS com Command/Query/Event/Behavior por funcionalidade no projeto Services
- AutoMapper é removido; mapeamento feito manualmente nos arquivos `*Map.cs`
- EF Core pode ser mantido apenas para migrations; queries passam para SQL puro via `IDataConfiguration`

**Frontend (Angular)**
- `site/cashflow/` e `site/cashflow2/` serão removidos
- `site/acme.sistemas.atena.mvc.site/` será removido
- `site/coreui-free-angular-admin-template/` é o único frontend; atualizar para Angular 17+

**Banco de dados**
- Adicionar coluna `tenant_id` em todas as tabelas existentes
- Novas tabelas: `tenants`, `roles`, `permissions`, `role_permissions`, `api_keys`, `refresh_tokens`, `token_blacklist`, `audit_logs`, `api_request_audit`, `plano_de_contas`, `centro_de_custo`, `funcionarios`, `nfe`, `nfe_itens`, `nfe_eventos`

**Integrações externas novas**
- SEFAZ (NF-e): webservice estadual por UF + SVRS contingência
- Certificado digital: integração com Bouncy Castle / DotNetty para A1 (pfx) e A3 (token)
- ViaCEP: já existente, manter
- Correios: já existente, manter

**Infraestrutura**
- Adicionar Redis para cache de tenant/permissão
- Adicionar RabbitMQ para emissão assíncrona de NF-e e envio de e-mail
- Adicionar armazenamento de arquivos (S3 ou Azure Blob) para XMLs de NF-e e DANFEs
