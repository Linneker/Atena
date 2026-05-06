## Context

O Atena é um ERP SaaS multi-tenant em construção. O backend atual usa Clean Architecture com 7 projetos `acme.atena.*`, EF Core para queries, AutoMapper e Controllers MVC. Despesa e Receita têm lógica real; os demais módulos são scaffolded. Há três frontends Angular paralelos sem unificação. Não há NF-e, RBAC, multi-tenancy real, auditoria, Redis ou mensageria.

O blueprint Acme (documentacao/blueprint.yml) define o padrão arquitetural esperado: Minimal API, CQRS com Command/Query/Event por funcionalidade, SQL puro nos repositórios e infra separada para banco, cache, storage e mensageria.

## Goals / Non-Goals

**Goals:**
- Migrar a estrutura de projetos para o padrão Acme blueprint
- Implementar multi-tenancy via `tenant_id` em todas as entidades e repositórios
- Completar todos os módulos ERP (financeiro, compras, vendas, estoque, cadastros, fiscal, relatórios)
- Emitir NF-e modelo 55 via SEFAZ com certificado digital
- Consolidar frontend em um único Angular/CoreUI
- Implementar RBAC granular com Roles e Permissions
- Auditoria imutável de todas as mutações

**Non-Goals:**
- NFS-e (nota de serviço) — fora do escopo desta fase
- Folha de pagamento e RH completo — apenas cadastro básico de funcionários
- Contabilidade fiscal completa (SPED, ECD, ECF) — apenas DRE e Balanço gerencial
- App mobile nativo — apenas responsividade no Angular
- Integração com marketplace (Shopee, Mercado Livre) — fase futura
- Multi-banco de dados por tenant (todos no mesmo MySQL)

## Decisions

### D1: Migração incremental, não big bang
**Decisão**: Migrar módulo a módulo mantendo a API existente em paralelo durante a transição, em vez de reescrever tudo de uma vez.
**Rationale**: O risco de uma reescrita total é muito alto. Migrar módulo a módulo permite testar cada parte isoladamente e manter o sistema funcionando.
**Alternativa considerada**: Reescrever do zero em branch separada — descartado por risco de divergência e tempo.

### D2: EF Core mantido apenas para migrations; SQL puro para queries
**Decisão**: Manter o `DbContext` do EF Core somente para migrations versionadas. Todas as queries de leitura e escrita usam `IDataConfiguration` (SQL puro via Dapper-like).
**Rationale**: O blueprint Acme define SQL puro como padrão. Elimina o N+1 implícito do EF Core e torna as queries auditáveis e otimizáveis.
**Alternativa considerada**: Manter EF Core para tudo — descartado por conflito com o blueprint e performance em queries complexas.

### D3: Isolamento de tenant via middleware, não via herança de entidade
**Decisão**: Um middleware extrai o `tenant_id` do JWT e injeta em `ITenantContext` (scoped). Os repositórios recebem `ITenantContext` via DI e aplicam o filtro em todas as queries automaticamente.
**Rationale**: Centralizar o filtro no repositório evita que um Handler esqueça de filtrar. O middleware garante que nenhuma requisição opera sem `tenant_id` (exceto endpoints de criação de tenant).
**Alternativa considerada**: Global query filter do EF Core — descartado por migrar para SQL puro.

### D4: NF-e via biblioteca NFeio/DanNFe + transmissão assíncrona via RabbitMQ
**Decisão**: Usar biblioteca .NET especializada (NFeio ou similar) para montar e assinar o XML NF-e. A transmissão à SEFAZ ocorre assíncrona via RabbitMQ — o faturamento não bloqueia na emissão.
**Rationale**: A SEFAZ pode ter latência. Bloquear o faturamento na resposta da SEFAZ é UX ruim. O usuário fatura instantaneamente; a NF-e é emitida em background com notificação de sucesso/falha.
**Alternativa considerada**: Transmissão síncrona — descartado por latência da SEFAZ (pode ser 2-10s).

### D5: RBAC com permissões no JWT, não consultadas a cada requisição
**Decisão**: As permissões da role do usuário são embutidas no JWT como claims ao fazer login. O middleware de autorização lê as claims do JWT sem consultar o banco.
**Rationale**: Elimina round-trip ao banco a cada requisição para verificar permissão. O JWT tem TTL curto (15min); o refresh token rotaciona.
**Alternativa considerada**: Consultar permissões no banco a cada request com cache Redis — viável mas mais complexo; o JWT é suficiente para o modelo atual.
**Trade-off**: Mudanças de permissão levam até 15min para fazer efeito (TTL do JWT). Mitigação: endpoint de revogação de token adiciona ao blacklist no Redis.

### D6: Frontend Angular 17+ com standalone components e signals
**Decisão**: Atualizar o CoreUI de Angular 14 para Angular 17+. Usar standalone components (sem NgModules), signals para state management e defer blocks para lazy loading.
**Rationale**: Angular 17 é o mais moderno disponível. Signals eliminam RxJS boilerplate para estado local. Standalone simplifica a arquitetura.
**Alternativa considerada**: Manter Angular 14 com NgModules — descartado por dívida técnica futura.

### D7: Storage de XMLs de NF-e em S3 compatível (MinIO para dev, AWS S3 para prod)
**Decisão**: XMLs autorizados são armazenados em S3 com path `{tenant_id}/{ano}/{mes}/{chave}.xml`. Em desenvolvimento, usar MinIO (container Docker). Em produção, AWS S3 ou Azure Blob.
**Rationale**: Obrigação legal de 5 anos de armazenamento. S3 é durável, barato e escalável. O blueprint já tem `GedAwsS3StorageProvider` e `GedAzureBlobStorageProvider`.

### D8: Cache Redis para tenant config e permissões
**Decisão**: Configurações de tenant (branding, limites de plano) e permissões de roles são cacheadas no Redis com TTL de 5 minutos.
**Rationale**: Essas informações mudam raramente mas são lidas em toda requisição. O cache evita consultas desnecessárias ao banco.

## Risks / Trade-offs

| Risco | Mitigação |
|---|---|
| NF-e SEFAZ tem comportamento diferente por UF | Testar com as principais UFs em homologação antes de produção; usar SVRS como contingência universal |
| Migração incremental gera período com dois padrões de código coexistindo | Definir módulos completos como unidade de migração; nunca deixar um módulo meio migrado |
| Certificado digital A1 vencido bloqueia emissão | Alertar 30 dias antes do vencimento; o sistema registra a data de expiração ao importar o certificado |
| Multi-tenancy adicionado retroativamente ao banco (dados sem tenant_id) | Migration adiciona `tenant_id` com o tenant existente como padrão; validar integridade antes de ativar o filtro |
| Permissões no JWT levam 15min para revogar | Endpoint de logout adiciona o JTI do token à blacklist no Redis; endpoints críticos consultam a blacklist |
| Complexidade do NF-e pode atrasar outros módulos | NF-e é implementada em sprint dedicada após módulo de Vendas estar funcional; as Vendas podem funcionar sem NF-e inicialmente |

## Migration Plan

### Fase 0 — Fundação (pré-requisito para tudo)
1. Criar nova estrutura de projetos Acme no mesmo repositório (pasta `src2/` temporária)
2. Migrar `acme.atena.core` → `Acme.Sistemas.Core` (Mediator, contratos, segurança)
3. Migrar `acme.atena.domain` → `Acme.Sistemas.Domain` (entidades, interfaces)
4. Adicionar coluna `tenant_id` em todas as tabelas via migration
5. Implementar `ITenantContext` e middleware de extração do JWT
6. Implementar RBAC (Roles, Permissions, tabelas e endpoints)

### Fase 1 — Módulos com lógica existente
7. Migrar Despesa e Receita para o padrão Acme (Command/Query/Handler/Behavior)
8. Migrar Empresa, Usuário e Endereço
9. Migrar autenticação para o novo padrão RBAC
10. Consolidar frontend CoreUI com Angular 17; criar shell e módulo de auth

### Fase 2 — Módulos scaffolded → implementação real
11. Implementar Dívida, Pagamento, Fluxo de Caixa, Contas a Pagar/Receber
12. Implementar Fornecedor, Cliente, Funcionário, Centro de Custo, Plano de Contas
13. Implementar Estoque (endpoints REST, entrada/saída, inventário)
14. Implementar Compras (solicitação → pedido → recebimento → conta a pagar)
15. Implementar Vendas (orçamento → pedido → faturamento → conta a receber)

### Fase 3 — Fiscal e relatórios
16. Implementar NF-e (configuração fiscal, emissão, DANFE, cancelamento, CC-e)
17. Implementar Dashboard e Relatórios
18. Implementar Auditoria e Conciliação Bancária

### Fase 4 — Encerramento
19. Remover projetos `acme.atena.*` legados
20. Remover frontends cashflow/, cashflow2/, MVC site
21. Testes de integração E2E cobrindo os fluxos principais

**Rollback**: Cada fase é independente. Se uma fase falhar, os projetos legados ainda funcionam até a fase anterior estar estável.

## Open Questions

- **Certificado A3**: O tenant pode usar token/smart card A3? Requer hardware no servidor — provavelmente não para SaaS; manter apenas A1 (PFX).
- **Emissão de NFS-e**: Há demanda dos clientes para emissão de nota de serviço? Se sim, em qual fase?
- **Boletos bancários**: Integração com bancos para geração de boleto registrado está no escopo desta proposta ou é fase futura?
- **Plano de Contas padrão**: O sistema deve disponibilizar um plano de contas padrão (Plano de Contas Referencial da RFB) para o tenant importar na criação?
- **Idioma**: O sistema precisa suportar múltiplos idiomas (i18n) ou apenas Português do Brasil?
- **Relatórios assíncronos**: Relatórios pesados (DRE anual, exportação grande) devem ser gerados em background com notificação de conclusão, ou o frontend espera?
