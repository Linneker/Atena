## 1. Fase 0 — Fundação Arquitetural (Acme Blueprint)

- [x] 1.1 Criar estrutura de pastas padrão Acme: `src/Api/`, `src/Service/`, `src/Data/`, `test/Unit/`, `test/Integration/`
- [x] 1.2 Criar projeto `Acme.Sistemas.Atena.Api` (Minimal API, .NET 8) com `Program.cs`, `IEndpoint`, estrutura de pastas `Endpoints/V1/`
- [x] 1.3 Criar projeto `Acme.Sistemas.Services` com `ServicesServiceCollection.cs` e estrutura `V1/`
- [x] 1.4 Criar projeto `Acme.Sistemas.Core` com Mediator próprio, contratos `IRequest`, `IRequestHandler`, `INotification`, `IPipelineBehavior`, `ResponseDefault`, helpers de segurança (`Hash`, `JwtOptions`, `PasswordHelper`)
- [x] 1.5 Criar projeto `Acme.Sistemas.Domain` com `BaseEntity.cs`, `ObjectCreateDelete.cs` e pastas `Entities/`, `Enums/`, `Interfaces/Repository/`
- [x] 1.6 Criar projeto `Acme.Sistemas.Repository` com `IDataConfiguration`, `ConvertExtensions` e `RepositoryServiceCollectionExtensions`
- [x] 1.7 Criar projeto `Acme.Sistemas.Infrastructure` com DatabaseContext, MigrationRunner, CacheStore (Redis), EmailQueueService, RabbitMqBus, GED providers (S3/local)
- [x] 1.8 Criar projeto `Acme.Sistemas.ExternalIntegration` com HttpClientProxy, ViaCEP client e ExternalIntegrationDI
- [x] 1.9 Criar projetos de teste `Acme.Sistemas.Services.UnitTest` (xUnit + Moq + Bogus) e `Acme.Sistemas.IntegrationTest` (WebApplicationFactory + Docker)
- [x] 1.10 Adicionar todos os projetos à solução `Atena.sln` e configurar referências entre projetos

## 2. Fase 0 — Multi-Tenancy e Banco de Dados

- [x] 2.1 Criar migration para adicionar coluna `tenant_id UUID NOT NULL` em todas as tabelas existentes (Despesa, Receita, Divida, Pagamento, FluxoDeCaixa, Empresa, Fornecedor, Produto, Compra, Venda, etc.)
- [x] 2.2 Criar tabela `tenants` (id, razao_social, cnpj, plano, status, logo_url, cor_primaria, fuso_horario, created_at)
- [x] 2.3 Criar tabela `tenant_limites` (tenant_id, max_usuarios, max_nfe_mes, max_storage_gb)
- [x] 2.4 Implementar `ITenantContext` e `TenantContextAccessor` (scoped) com extração de `tenant_id` do JWT
- [x] 2.5 Implementar middleware `TenantMiddleware` que valida e injeta o `ITenantContext` em cada requisição
- [x] 2.6 Implementar `TenantRepository` com queries de CRUD e consulta por CNPJ
- [x] 2.7 Implementar endpoints V1 de gestão de tenants (`POST /api/v1/tenants`, `GET`, `PUT`, `DELETE`) — acesso restrito a super-admin
- [x] 2.8 Garantir que todos os repositórios aplicam filtro `WHERE tenant_id = @tenantId` automaticamente via base class `BaseRepository`

## 3. Fase 0 — Segurança RBAC

- [x] 3.1 Criar tabelas: `roles`, `permissions`, `role_permissions`, `user_roles`, `api_keys`, `refresh_tokens`, `token_blacklist`
- [x] 3.2 Definir todas as permissões do sistema (recurso + ação) como constantes em `Acme.Sistemas.Core/Const/Permissions.cs`
- [x] 3.3 Implementar `RoleEntity`, `PermissionEntity`, `RolePermissionEntity` no Domain
- [x] 3.4 Implementar repositórios: `RoleRepository`, `PermissionRepository`, `RolePermissionRepository`, `ApiKeyRepository`, `RefreshTokenRepository`, `TokenBlacklistRepository`
- [x] 3.5 Implementar `V1/Autenticacao/Command/Login` — gera JWT com permissões nas claims + refresh token
- [x] 3.6 Implementar `V1/Autenticacao/Command/RenovarToken` — troca refresh token rotacionado por novo JWT
- [x] 3.7 Implementar `V1/Autenticacao/Command/Logout` — adiciona refresh token à blacklist
- [x] 3.8 Implementar endpoints de gestão de roles e permissões (CRUD de roles, atribuição de permissões, atribuição de roles a usuários)
- [x] 3.9 Implementar `PermissaoAttribute` ou Minimal API authorization policy que verifica claims do JWT
- [x] 3.10 Implementar verificação de blacklist no middleware de autenticação para tokens revogados
- [x] 3.11 Implementar bloqueio de conta após 5 tentativas de login consecutivas com desbloqueio automático (15min)
- [x] 3.12 Criar role padrão "Administrador" com todas as permissões no seed de criação de tenant

## 4. Fase 1 — Migração: Módulo Financeiro (Despesa e Receita)

- [ ] 4.1 Migrar entidades `Despesa` e `Receita` para `Acme.Sistemas.Domain/Entities/Financeiro/`
- [ ] 4.2 Criar `V1/Despesa/Command/CriarDespesa` (Command, Handler, Behavior, Result, Validation)
- [ ] 4.3 Criar `V1/Despesa/Command/AlterarDespesa` (Command, Handler, Behavior, Result, Validation)
- [ ] 4.4 Criar `V1/Despesa/Command/ExcluirDespesa` (Command, Handler, Behavior, Result, Validation)
- [ ] 4.5 Criar `V1/Despesa/Command/BaixarDespesa` — registra pagamento e atualiza FluxoDeCaixa
- [ ] 4.6 Criar `V1/Despesa/Query/ListarDespesas` (Query, Handler, Behavior, Result — com filtros de competência, status, categoria)
- [ ] 4.7 Criar `V1/Despesa/Query/ObterDespesa` (por ID)
- [ ] 4.8 Criar `DespesaRepository` com queries SQL puras e `DespesaQuery.cs`
- [ ] 4.9 Criar endpoints Minimal API em `Endpoints/V1/Despesa/` mapeando para Commands/Queries
- [ ] 4.10 Repetir itens 4.2–4.9 para `Receita`
- [ ] 4.11 Implementar `V1/FluxoDeCaixa/Query/ObterFluxo` — consolida receitas e despesas do período
- [ ] 4.12 Implementar `V1/FluxoDeCaixa/Command/FecharPeriodo` — fecha competência como imutável

## 5. Fase 1 — Migração: Usuário e Empresa

- [ ] 5.1 Migrar entidades `Usuario`, `Empresa`, `Endereco` para o Domain
- [ ] 5.2 Criar `V1/Usuario/Command/CriarUsuario`, `AlterarUsuario`, `ExcluirUsuario` com validações
- [ ] 5.3 Criar `V1/Usuario/Query/ListarUsuarios`, `ObterUsuario`
- [ ] 5.4 Criar `UsuarioRepository` com SQL puro
- [ ] 5.5 Criar `V1/Empresa/Command/CriarEmpresa`, `AlterarEmpresa` com validação de CNPJ e busca de endereço via CEP
- [ ] 5.6 Criar endpoints Minimal API para Usuário e Empresa

## 6. Fase 2 — Módulo Financeiro Completo

- [ ] 6.1 Criar entidades `Divida`, `Pagamento`, `ContaPagar`, `ContaReceber`, `ConciliacaoBancaria` no Domain
- [ ] 6.2 Implementar `V1/Divida` — CRUD completo com Command/Query/Handler/Behavior
- [ ] 6.3 Implementar `V1/ContaPagar` — criar, baixar (total/parcial), alerta de vencimento
- [ ] 6.4 Implementar `V1/ContaReceber` — criar, receber, controle de inadimplência
- [ ] 6.5 Implementar `V1/ConciliacaoBancaria/Command/ImportarExtrato` — processamento de OFX/CSV
- [ ] 6.6 Implementar algoritmo de conciliação automática por valor e data
- [ ] 6.7 Implementar `V1/PlanoDeContas` — CRUD hierárquico com validação de pai/filho
- [ ] 6.8 Implementar `V1/CentroDeCusto` — CRUD com vínculo a despesas/receitas
- [ ] 6.9 Implementar `V1/Relatorios/Financeiro/DRE` — geração baseada no plano de contas
- [ ] 6.10 Implementar `V1/Relatorios/Financeiro/Balanco` — balanço patrimonial gerencial
- [ ] 6.11 Implementar geração de PDF de relatórios (DRE, Balanço) com branding do tenant

## 7. Fase 2 — Módulo de Cadastros Completo

- [ ] 7.1 Criar entidades `Cliente`, `Fornecedor`, `Funcionario`, `PlanoDeContas`, `CentroDeCusto` no Domain
- [ ] 7.2 Implementar `V1/Cliente` — CRUD com validação CPF/CNPJ, busca CEP, controle inadimplência
- [ ] 7.3 Implementar `V1/Fornecedor` — CRUD com vínculo de produtos e condições de pagamento padrão
- [ ] 7.4 Implementar `V1/Funcionario` — CRUD básico com cargo, departamento e centro de custo
- [ ] 7.5 Implementar `V1/Produto` — CRUD com código de barras, unidade de medida, tabela de preços multi-nível
- [ ] 7.6 Implementar `V1/TipoProduto` e `V1/TipoValorProduto` (tipos de preço)

## 8. Fase 2 — Módulo de Estoque

- [ ] 8.1 Criar entidades `Estoque`, `EstoqueProduto`, `EntradaProdutoEstoque`, `SaidaProdutoEstoque`, `Inventario` no Domain
- [ ] 8.2 Criar repositórios SQL para todas as entidades de estoque
- [ ] 8.3 Implementar `V1/Estoque/Query/ConsultarSaldo` — saldo disponível, reservado e total por produto
- [ ] 8.4 Implementar `V1/Estoque/Command/RegistrarEntrada` — entrada manual com motivo
- [ ] 8.5 Implementar `V1/Estoque/Command/RegistrarSaida` — saída manual com bloqueio de saldo negativo (configurável)
- [ ] 8.6 Implementar custeio FIFO — calcular CMV na saída com base nos lotes mais antigos
- [ ] 8.7 Implementar `V1/Inventario/Command/AbrirInventario` — bloqueia movimentações dos produtos
- [ ] 8.8 Implementar `V1/Inventario/Command/FecharInventario` — gera ajustes automáticos das diferenças
- [ ] 8.9 Implementar `V1/Estoque/Query/RelatorioMovimentacao` — extrato de movimentação por produto e período
- [ ] 8.10 Implementar alerta de estoque mínimo — `Event/AlertaEstoqueMinimo` disparado quando saldo < mínimo
- [ ] 8.11 Criar endpoints Minimal API V1 para todos os recursos de estoque

## 9. Fase 2 — Módulo de Compras

- [ ] 9.1 Criar entidades `SolicitacaoCompra`, `PedidoCompra`, `PedidoCompraItem`, `RecebimentoCompra` no Domain
- [ ] 9.2 Implementar `V1/SolicitacaoCompra` — CRUD com fluxo de aprovação (status: rascunho → aguardando aprovação → aprovada/rejeitada)
- [ ] 9.3 Implementar `V1/SolicitacaoCompra/Command/Aprovar` e `Rejeitar` com alçada por valor
- [ ] 9.4 Implementar `V1/PedidoCompra/Command/Criar` — a partir de solicitação aprovada ou direto
- [ ] 9.5 Implementar `V1/PedidoCompra/Command/EnviarFornecedor` — gera PDF e envia por e-mail
- [ ] 9.6 Implementar `V1/RecebimentoCompra/Command/Registrar` — total, parcial ou com divergência; gera entrada de estoque e conta a pagar
- [ ] 9.7 Implementar `V1/RecebimentoCompra/Command/VincularNFe` — valida chave de acesso na SEFAZ
- [ ] 9.8 Implementar notificações de aprovação pendente (`Event/NotificarAprovacaoPendente`)
- [ ] 9.9 Criar endpoints Minimal API V1 para Compras

## 10. Fase 2 — Módulo de Vendas

- [ ] 10.1 Criar entidades `Orcamento`, `PedidoVenda`, `PedidoVendaItem`, `Faturamento`, `DevolucaoVenda`, `ComissaoVendedor` no Domain
- [ ] 10.2 Implementar `V1/Orcamento` — CRUD com validade e conversão para pedido
- [ ] 10.3 Implementar `V1/PedidoVenda/Command/Criar` — verificação de estoque, reserva e desconto por alçada
- [ ] 10.4 Implementar `V1/PedidoVenda/Command/Confirmar` — reserva estoque definitivamente
- [ ] 10.5 Implementar `V1/Faturamento/Command/Faturar` — total ou parcial; baixa estoque, gera conta a receber e dispara emissão NF-e
- [ ] 10.6 Implementar `V1/DevolucaoVenda/Command/Registrar` — total ou parcial; retorna estoque, estorna conta a receber e dispara NF-e de devolução
- [ ] 10.7 Implementar cálculo de comissão de vendedor no evento de faturamento
- [ ] 10.8 Implementar `V1/Relatorios/Vendas` — por vendedor, cliente, produto e período
- [ ] 10.9 Criar endpoints Minimal API V1 para Vendas

## 11. Fase 3 — Fiscal NF-e

- [ ] 11.1 Criar entidades `ConfiguracaoFiscal`, `NFe`, `NFeItem`, `NFeEvento` no Domain
- [ ] 11.2 Implementar `V1/ConfiguracaoFiscal/Command/ImportarCertificado` — upload e armazenamento criptografado do PFX A1
- [ ] 11.3 Implementar `V1/ConfiguracaoFiscal/Command/AlterarAmbiente` — troca homologação/produção
- [ ] 11.4 Integrar biblioteca de geração/assinatura de XML NF-e (NFeio ou nfe-net)
- [ ] 11.5 Implementar `V1/NFe/Command/EmitirNFe` — monta XML, assina, transmite via RabbitMQ (assíncrono)
- [ ] 11.6 Implementar worker `NFeTransmissaoWorker` — consome fila RabbitMQ, transmite à SEFAZ, atualiza status e armazena XML no S3
- [ ] 11.7 Implementar modo de contingência SVRS — ativado automaticamente em falha do webservice principal
- [ ] 11.8 Implementar geração de DANFE em PDF após autorização
- [ ] 11.9 Implementar envio automático de DANFE por e-mail ao cliente (via EmailQueueService)
- [ ] 11.10 Implementar `V1/NFe/Command/CancelarNFe` — valida prazo 24h, transmite evento de cancelamento, reverte estoque e conta a receber
- [ ] 11.11 Implementar `V1/NFe/Command/EmitirCCe` — carta de correção eletrônica
- [ ] 11.12 Implementar armazenamento de XMLs no S3 com path `{tenant_id}/{ano}/{mes}/{chave}.xml`
- [ ] 11.13 Implementar alerta de certificado a vencer (30 dias antes)
- [ ] 11.14 Implementar controle de limite de NF-e por plano de tenant

## 12. Fase 3 — Dashboard e Relatórios

- [ ] 12.1 Implementar `V1/Dashboard/Query/ObterKpis` — receita, despesa, resultado, vendas abertas, vencimentos, estoque crítico
- [ ] 12.2 Implementar `V1/Dashboard/Query/EvolucaoFinanceira` — receitas vs despesas últimos 12 meses
- [ ] 12.3 Implementar `V1/Relatorios/ContasPagar/Aging` — por faixas de vencimento
- [ ] 12.4 Implementar `V1/Relatorios/ContasReceber/Aging` — por faixas de vencimento
- [ ] 12.5 Implementar `V1/Relatorios/Estoque/Posicao` — saldo e valor por produto
- [ ] 12.6 Implementar exportação de relatórios para Excel (NPOI ou EPPlus) e PDF (DinkToPdf ou QuestPDF)

## 13. Fase 3 — Auditoria

- [ ] 13.1 Criar tabelas `audit_logs` e `api_request_audit` no banco
- [ ] 13.2 Implementar `AuditLogEntity` e `ApiRequestAuditEntity` no Domain
- [ ] 13.3 Implementar `AuditBehavior` no pipeline do Mediator — captura dados antes/depois em Commands de escrita
- [ ] 13.4 Implementar middleware `ApiRequestAuditMiddleware` — registra toda requisição de forma assíncrona
- [ ] 13.5 Implementar `V1/Auditoria/Query/ListarLogs` — com filtros por usuário, entidade, operação e período (apenas admins)
- [ ] 13.6 Implementar `V1/Auditoria/Query/HistoricoRegistro` — histórico de alterações de um registro específico por ID
- [ ] 13.7 Implementar exportação de logs de auditoria em JSON com hash de integridade SHA-256

## 14. Fase 4 — Frontend CoreUI Angular 17+

- [ ] 14.1 Atualizar CoreUI de Angular 14 para Angular 17 com standalone components e signals
- [ ] 14.2 Implementar `TenantBrandingService` — carrega logo e cores do tenant pós-login e aplica via CSS custom properties
- [ ] 14.3 Implementar `AuthStore` (signal-based) com JWT, permissões e refresh automático de token
- [ ] 14.4 Implementar guard `PermissaoGuard` — verifica permissão por rota
- [ ] 14.5 Implementar diretiva `*temPermissao` — oculta elementos de UI sem a permissão necessária
- [ ] 14.6 Criar módulo `FinanceiroModule` — telas de Despesa, Receita, Fluxo de Caixa, Contas a Pagar/Receber, Conciliação Bancária
- [ ] 14.7 Criar módulo `CadastrosModule` — telas de Cliente, Fornecedor, Funcionário, Produto, Centro de Custo, Plano de Contas
- [ ] 14.8 Criar módulo `EstoqueModule` — telas de Saldo, Movimentação, Inventário
- [ ] 14.9 Criar módulo `ComprasModule` — telas de Solicitação, Pedido de Compra, Recebimento
- [ ] 14.10 Criar módulo `VendasModule` — telas de Orçamento, Pedido de Venda, Faturamento, Devoluções
- [ ] 14.11 Criar módulo `FiscalModule` — telas de Configuração Fiscal, NF-e (listagem, detalhes, cancelamento, CC-e)
- [ ] 14.12 Criar módulo `RelatoriosModule` — Dashboard, DRE, Balanço, Aging, Relatório de Vendas/Estoque
- [ ] 14.13 Criar módulo `ConfiguracaoModule` — Usuários, Roles, Permissões, Parâmetros, Branding do Tenant
- [ ] 14.14 Implementar tabelas genéricas com paginação server-side, filtro com debounce e ordenação
- [ ] 14.15 Implementar `NotificacaoService` — polling ou WebSocket para notificações em tempo real
- [ ] 14.16 Implementar exportação para Excel em todas as listagens (SheetJS/xlsx)
- [ ] 14.17 Garantir responsividade em mobile (360px) e tablet (768px) para todos os módulos

## 15. Fase 4 — Encerramento e Limpeza

- [ ] 15.1 Remover projetos legados `acme.atena.*` da solução após todos os módulos migrarem
- [ ] 15.2 Remover frontends `site/cashflow/`, `site/cashflow2/` e `site/acme.sistemas.atena.mvc.site/`
- [ ] 15.3 Atualizar `CLAUDE.md` com nova estrutura de projetos, comandos e arquitetura
- [ ] 15.4 Escrever testes unitários para todos os Handlers e Behaviors críticos (cobertura mínima 70%)
- [ ] 15.5 Escrever testes de integração E2E para os fluxos principais: login → venda → faturamento → NF-e, compra → recebimento → estoque → conta a pagar
- [ ] 15.6 Configurar docker-compose.yml com todos os serviços: API, MySQL, Redis, RabbitMQ, MinIO
- [ ] 15.7 Configurar manifesto Kubernetes em `infra/k8s/v1/` com deployment, service e configmaps
- [ ] 15.8 Validar que nenhum dado de um tenant é acessível por outro (teste de isolamento cross-tenant)
