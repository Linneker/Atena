# Estrutura Padrão de Projetos Acme Sistemas

Este documento define o modelo padrão de estrutura para todos os projetos de desenvolvimento da plataforma Acme. Serve como referência arquitetural, de organização de pastas, e de responsabilidade de cada componente.

---

## Estrutura de Diretórios da Solução

```
PastaProjeto/
├── infra/
│   ├── compose/
│   │   └── docker-compose.yml
│   └── k8s/
│       ├── v1/
│       │   └── deployment.yaml
│       └── kind-config.yaml
├── src/
│   ├── Api/
│   │   └── Acme.Sistemas.NomeProjeto.Api       (projeto web.api)
│   ├── Service/
│   │   ├── Acme.Sistemas.Services               (projeto biblioteca de classe)
│   │   ├── Acme.Sistemas.Core                   (projeto biblioteca de classe)
│   │   └── Acme.Sistemas.Domain                 (projeto biblioteca de classe)
│   └── Data/
│       ├── Acme.Sistemas.ExternalIntegration    (projeto biblioteca de classe)
│       ├── Acme.Sistemas.Infrastructure         (projeto biblioteca de classe)
│       └── Acme.Sistemas.Repository             (projeto biblioteca de classe)
└── test/
    ├── Integration/
    │   └── Acme.Sistemas.IntegrationTest        (projeto de teste)
    └── Unit/
        └── Acme.Sistemas.Services.UnitTest      (projeto de teste)
```

> Tanto pastas virtuais (agrupamentos lógicos na solução) quanto pastas físicas (diretórios no disco) devem ser mantidas.

---

## Projetos da Solução

---

### Acme.Sistemas.NomeProjeto.Api

Camada de entrada da aplicação. Responsável por expor os endpoints HTTP da API. Utiliza Minimal API com arquivos separados por endpoint.

```
Acme.Sistemas.NomeProjeto.Api/
├── Endpoints/
│   └── V{N}/
│       └── {NomeEndpoint}/
│           ├── {NomeArquivo}.cs
│           ├── {NomeArquivo}Request.cs          (opcional — pode não existir em GETs simples)
│           ├── {NomeArquivo}Response.cs
│           └── {NomeArquivo}Map.cs
├── Middlewares/
├── Hosted/
├── Config/
├── cache.db
├── featureflags.json
├── Dockerfile
└── Program.cs
```

#### Pastas

| Pasta | Descrição |
|---|---|
| `Endpoints/` | Contém todos os Minimal API endpoints, organizados por versão (`V1`, `V2`, ...) e depois por nome do endpoint |
| `Endpoints/V{N}/{NomeEndpoint}/` | Conjunto de arquivos que compõem um endpoint específico |
| `Middlewares/` | Middlewares customizados da aplicação |
| `Hosted/` | Workers e serviços hospedados (IHostedService) |
| `Config/` | Configurações globais: segurança, retorno de erros, OpenAPI, etc. |

#### Arquivos por Endpoint

| Arquivo | Descrição |
|---|---|
| `{Nome}.cs` | Implementação do endpoint usando a interface `IEndpoint`. Define a rota, método HTTP e handler |
| `{Nome}Request.cs` | Contrato de entrada da requisição. Opcional em endpoints GET sem parâmetros complexos |
| `{Nome}Response.cs` | Contrato de saída. Define as propriedades retornadas ao consumidor da API |
| `{Nome}Map.cs` | Converte `Request → Command/Query` e `CommandResult/QueryResult → Response` |

#### Arquivos Raiz

| Arquivo | Descrição |
|---|---|
| `cache.db` | Banco de dados local LiteDB usado para cache local, como alternativa a Redis |
| `featureflags.json` | Arquivo de feature flags acessado via API para ativar/desativar funcionalidades em runtime |
| `Dockerfile` | Configuração de imagem Docker para o projeto |
| `Program.cs` | Ponto de entrada e configuração da aplicação |

---

### Acme.Sistemas.Services

Camada de aplicação. Contém toda a lógica de negócio organizada no padrão CQRS por funcionalidade.

```
Acme.Sistemas.Services/
├── V{N}/
│   └── {NomeDaFuncionalidade}/
│       ├── Command/
│       │   └── {AçãoEspecifica}/
│       │       ├── {Nome}Command.cs
│       │       ├── {Nome}CommandHandler.cs
│       │       ├── {Nome}CommandBehavior.cs
│       │       ├── {Nome}CommandResult.cs
│       │       └── {Nome}CommandValidation.cs
│       ├── Query/
│       │   └── {AçãoEspecifica}/
│       │       ├── {Nome}Query.cs
│       │       ├── {Nome}QueryHandler.cs
│       │       ├── {Nome}QueryBehavior.cs
│       │       ├── {Nome}QueryResult.cs
│       │       └── {Nome}QueryValidation.cs
│       ├── Event/
│       │   └── {AçãoEspecifica}/
│       │       ├── {Nome}Notification.cs
│       │       ├── {Nome}NotificationHandler.cs
│       │       ├── {Nome}NotificationBehavior.cs
│       │       └── {Nome}NotificationValidation.cs
│       └── Services/
│           ├── {Nome}Service.cs
│           └── I{Nome}Service.cs
└── ServicesServiceCollecation.cs
```

#### Organização por Versão

A pasta `V{N}` representa a versão da funcionalidade (`V1`, `V2`, ...). Cada versão pode coexistir sem quebrar contratos anteriores.

#### Command

Representa uma operação de escrita ou ação que altera estado.

| Arquivo | Descrição |
|---|---|
| `{Nome}Command.cs` | Record com as propriedades que serão passadas para executar a ação |
| `{Nome}CommandHandler.cs` | Lógica principal: orquestra chamadas a services e repositories |
| `{Nome}CommandBehavior.cs` | Comportamento do pipeline: logs, validações complementares, enriquecimento de dados |
| `{Nome}CommandResult.cs` | Contrato de retorno da operação |
| `{Nome}CommandValidation.cs` | Validações FluentValidation dos dados do Command |

#### Query

Representa uma operação de leitura de dados, sem efeito colateral.

| Arquivo | Descrição |
|---|---|
| `{Nome}Query.cs` | Record com as propriedades necessárias para consulta |
| `{Nome}QueryHandler.cs` | Lógica principal: chama repositories e monta o resultado |
| `{Nome}QueryBehavior.cs` | Comportamento do pipeline: logs, caching, enriquecimento |
| `{Nome}QueryResult.cs` | Contrato de retorno da consulta |
| `{Nome}QueryValidation.cs` | Validações dos dados da Query |

#### Event

Representa eventos internos de domínio disparados de forma assíncrona.

| Arquivo | Descrição |
|---|---|
| `{Nome}Notification.cs` | Record com as propriedades do evento |
| `{Nome}NotificationHandler.cs` | Handler que reage ao evento disparado |
| `{Nome}NotificationBehavior.cs` | Comportamento do pipeline do evento |
| `{Nome}NotificationValidation.cs` | Validações dos dados da Notification |

> Events não possuem retorno.

#### Services

| Arquivo | Descrição |
|---|---|
| `{Nome}Service.cs` | Implementação concreta da lógica de negócio |
| `I{Nome}Service.cs` | Interface que define o contrato do serviço |

#### Arquivo Raiz

| Arquivo | Descrição |
|---|---|
| `ServicesServiceCollecation.cs` | Registra todas as injeções de dependência do projeto .Services |

---

### Acme.Sistemas.Core

Projeto de infraestrutura transversal. Fornece os contratos, utilitários e o mecanismo de CQRS/Mediator utilizados por toda a solução.

```
Acme.Sistemas.Core/
├── Const/
│   └── KeyCache.cs
├── Erros/
│   └── MessageErros.cs
├── Helper/
│   ├── DynamicConverter.cs
│   └── LogEnrichmentHelper.cs
├── Mediators/
│   ├── Handler/
│   │   ├── IRequest.cs
│   │   └── IRequestHandler.cs
│   ├── Notification/
│   │   ├── INotification.cs
│   │   └── INotificationHandler.cs
│   ├── IMediator.cs
│   ├── IPipelineBehavior.cs
│   └── Mediator.cs
├── Response/
│   ├── Erros/
│   │   └── Error.cs
│   └── ResponseDefault.cs
├── Security/
│   ├── CryptographyAsync.cs
│   ├── Hash.cs
│   ├── JwtOptions.cs
│   └── PasswordHelper.cs
├── Settings/
│   ├── CacheMetrics.cs
│   ├── FeatureFlagSettings.cs
│   └── MemoryUsageStats.cs
└── DepencieInjection.cs
```

#### Pasta `Const/`

| Arquivo | Descrição |
|---|---|
| `KeyCache.cs` | Constantes de chaves de cache utilizadas em toda a aplicação. Exemplo: `"user:{id}"`, `"token:{id}"`. Centraliza os nomes das chaves para evitar strings duplicadas |

#### Pasta `Erros/`

| Arquivo | Descrição |
|---|---|
| `MessageErros.cs` | Constantes de mensagens de erro padronizadas. Evita strings avulsas espalhadas pelo código |

#### Pasta `Helper/`

| Arquivo | Descrição |
|---|---|
| `DynamicConverter.cs` | Classe estática utilitária para conversão universal de tipos. Suporta primitivos, enums, nullable types, usando `TypeConverter` com cache interno para performance |
| `LogEnrichmentHelper.cs` | Utilitário de logging. Adiciona contexto enriquecido aos logs, como identificadores de tenant, usuário, correlação de requisição |

#### Pasta `Mediators/`

Implementação própria do padrão Mediator/CQRS da plataforma.

**Subpasta `Handler/`**

| Arquivo | Tipo | Descrição |
|---|---|---|
| `IRequest.cs` | Interface genérica | Marcador para comandos e queries do CQRS. Define o tipo de retorno esperado: `IRequest<TResponse>` |
| `IRequestHandler.cs` | Interface genérica | Contrato do handler de um request específico. Um handler por tipo de request |

**Subpasta `Notification/`**

| Arquivo | Tipo | Descrição |
|---|---|---|
| `INotification.cs` | Interface | Marcador para eventos de domínio (publicações assíncronas). Não possui retorno |
| `INotificationHandler.cs` | Interface genérica | Contrato do handler de uma notification. Múltiplos handlers podem existir para o mesmo evento |

**Arquivos raiz de `Mediators/`**

| Arquivo | Tipo | Descrição |
|---|---|---|
| `IMediator.cs` | Interface | Contrato central do mediator. Expõe `Send<TResponse>(IRequest<TResponse>)` para comandos/queries e `Publish(INotification)` para eventos |
| `IPipelineBehavior.cs` | Interface genérica | Middleware do pipeline de execução de requests. Permite encadear comportamentos (logging, validação, cache) antes e depois do handler |
| `Mediator.cs` | Classe concreta | Implementação do IMediator. Usa reflexão com cache para descobrir `IRequestHandler` e `IPipelineBehavior` registrados no container de DI. Monta o pipeline de execução e despacha a chamada |

#### Pasta `Response/`

**Subpasta `Erros/`**

| Arquivo | Tipo | Descrição |
|---|---|---|
| `Error.cs` | Classe | Representa um erro com `Code` (identificador do erro) e `Message` (descrição legível). Usado dentro de `ResponseDefault` |

**Arquivo raiz**

| Arquivo | Tipo | Descrição |
|---|---|---|
| `ResponseDefault.cs` | Classe genérica | Wrapper padrão de resposta HTTP. Contém `StatusCode`, `Message`, `Content<T>` e lista de `Errors`. Fornece métodos de fábrica estáticos para respostas de sucesso e falha |

#### Pasta `Security/`

| Arquivo | Tipo | Descrição |
|---|---|---|
| `CryptographyAsync.cs` | Classe estática | Criptografia assimétrica RSA com padding OAEP+SHA512. Fornece métodos de criptografia e descriptografia assíncronos para dados sensíveis |
| `Hash.cs` | Classe estática | Funções de hash: SHA256 (retorna base64), SHA512 (retorna hex ou bytes), hash de streams de arquivo, e comparação segura contra timing attacks |
| `JwtOptions.cs` | Classe de configuração | Parâmetros JWT: `Issuer`, `Audience`, `Key`, TTL do token de acesso (minutos) e TTL do refresh token (dias) |
| `PasswordHelper.cs` | Classe estática | Valida senhas fortes: mínimo 12 caracteres, letras maiúsculas, minúsculas, dígitos, caracteres especiais e caracteres distintos. Retorna lista de violações |

#### Pasta `Settings/`

| Arquivo | Tipo | Descrição |
|---|---|---|
| `CacheMetrics.cs` | Classe | Rastreia métricas de performance do cache: hits, misses, evictions. Usado para monitoramento e diagnóstico |
| `FeatureFlagSettings.cs` | Classe | Configuração das feature flags. Mapeada do `featureflags.json` via `IOptions<T>` |
| `MemoryUsageStats.cs` | Classe | Dados de monitoramento de uso de memória da aplicação |

#### Arquivo Raiz

| Arquivo | Tipo | Descrição |
|---|---|---|
| `DepencieInjection.cs` | Classe estática de extensão | Registra o Mediator, handlers, behaviors e notifications no container de DI. Oferece três métodos: `AddCustomMediatorTransient`, `AddCustomMediatorScoped` e `AddCustomMediatorSingleton`, variando o lifetime conforme o caso de uso |

---

### Acme.Sistemas.Domain

Camada de domínio. Contém as entidades, enumerações, interfaces de repositório e constantes de negócio. Não possui dependências de infraestrutura.

```
Acme.Sistemas.Domain/
├── Constants/
│   ├── GedConstants.cs
│   └── ProtocolConstants.cs
├── Entities/
│   ├── Archives/
│   │   ├── FileProtocolEntity.cs
│   │   ├── FilesEntity.cs
│   │   └── FilesSolicitationEntity.cs
│   ├── Bpmn/
│   │   ├── BpmnBranchHistoryEntity.cs
│   │   ├── BpmnExecutionEntity.cs
│   │   ├── BpmnProcessEntity.cs
│   │   ├── BpmnProcessState.cs
│   │   └── BpmnSnapshot.cs
│   ├── Cache/
│   ├── Catalog/
│   ├── Configurations/
│   ├── Emails/
│   │   ├── EmailDispatchLogEntity.cs
│   │   ├── EmailProviderConfigEntity.cs
│   │   └── PasswordResetTokenEntity.cs
│   ├── Flows/
│   │   ├── BpmnTemplateEntity.cs
│   │   ├── FlowDefinitionEntity.cs
│   │   ├── ProtocolModelListItemEntity.cs
│   │   └── ProtocolModelMigrationPolicyEntity.cs
│   ├── Ged/
│   │   ├── GedDocumentEntity.cs
│   │   ├── GedDocumentClassProfileEntity.cs
│   │   ├── GedDocumentRetentionPolicyEntity.cs
│   │   ├── GedDocumentSignatureEntity.cs
│   │   ├── GedDocumentVersionEntity.cs
│   │   ├── GedDocumentAuditEntryEntity.cs
│   │   ├── GedDocumentProcessingJobEntity.cs
│   │   └── GedDocumentStatuses.cs
│   ├── IA/
│   │   ├── AiDecisionEntity.cs
│   │   ├── AiFeedbackEntity.cs
│   │   ├── AiSolicitationCaseEntity.cs
│   │   └── SolicitationAiAnalysisModels.cs
│   ├── Notifications/
│   ├── Organizations/
│   ├── Permissions/
│   │   ├── RoleEntity.cs
│   │   ├── PermissionEntity.cs
│   │   ├── RolePermissionEntity.cs
│   │   ├── ApiKeyEntity.cs
│   │   ├── RefreshTokenEntity.cs
│   │   └── TokenBlacklistEntity.cs
│   ├── Process/
│   │   ├── ProtocolEntity.cs
│   │   ├── ProtocolEventEntity.cs
│   │   ├── ProtocolIntegrationOutboxEntity.cs
│   │   ├── SolicitationEntity.cs
│   │   └── SolicitationProtocolTypeEntity.cs
│   ├── RuleEngine/
│   │   ├── RuleEngineEntities.cs
│   │   ├── RuleEngineDtos.cs
│   │   └── Contracts/
│   │       └── RuleEngineModelPayload.cs
│   ├── Tenants/
│   │   └── TenantBrandingEntity.cs
│   ├── Users/
│   ├── Others/
│   │   ├── AuditLogEntity.cs
│   │   └── ApiRequestAuditEntity.cs
│   ├── BaseEntity.cs
│   └── ObjectCreateDelete.cs
├── Enums/
└── Interfaces/
    └── Repository/
```

#### Pasta `Constants/`

| Arquivo | Descrição |
|---|---|
| `GedConstants.cs` | Constantes de negócio do módulo GED (Gestão Eletrônica de Documentos): tipos de documento, tamanhos máximos, extensões permitidas, etc. |
| `ProtocolConstants.cs` | Constantes do módulo de protocolos: prazos padrão, status, categorias de eventos |

#### Pasta `Entities/`

Contém todas as entidades do domínio. Cada subpasta agrupa entidades de um módulo específico.

**Exemplos**:
**Archives** — Arquivos vinculados a processos e solicitações

| Arquivo | Descrição |
|---|---|
| `FilesEntity.cs` | Entidade base de arquivo. Armazena metadados como nome, tipo MIME, tamanho e caminho |
| `FileProtocolEntity.cs` | Associação entre um arquivo e um protocolo de processo |
| `FilesSolicitationEntity.cs` | Associação entre um arquivo e uma solicitação |

**Bpmn** — Motor de processos BPMN em runtime

| Arquivo | Descrição |
|---|---|
| `BpmnProcessEntity.cs` | Instância de um processo BPMN em execução. Armazena estado atual, nó ativo e histórico |
| `BpmnExecutionEntity.cs` | Registro de uma execução individual de um passo do processo |
| `BpmnBranchHistoryEntity.cs` | Histórico de bifurcações paralelas (fork/join) de um processo |
| `BpmnProcessState.cs` | Enumeração dos estados possíveis de um processo BPMN |
| `BpmnSnapshot.cs` | Captura imutável da definição do fluxo BPMN no momento da instanciação |

**Cache** — Entidades relacionadas ao controle de cache distribuído

**Catalog** — Entidades do catálogo de produtos ou serviços

**Configurations** — Entidades de configuração de menus de navegação, views e permissões de interface

**Emails** — Comunicação por e-mail e recuperação de senha

| Arquivo | Descrição |
|---|---|
| `EmailDispatchLogEntity.cs` | Log de envios de e-mail, incluindo status e timestamp |
| `EmailProviderConfigEntity.cs` | Configurações de provedor de e-mail por tenant (SMTP, SendGrid, etc.) |
| `PasswordResetTokenEntity.cs` | Token temporário para fluxo de recuperação de senha |

**Flows** — Definições de fluxo de processo

| Arquivo | Descrição |
|---|---|
| `FlowDefinitionEntity.cs` | Definição completa de um fluxo BPMN armazenado. É o template a partir do qual processos são instanciados |
| `BpmnTemplateEntity.cs` | Template base de BPMN para criação de novos modelos de protocolo |
| `ProtocolModelListItemEntity.cs` | Item de listagem de modelos de protocolo disponíveis |
| `ProtocolModelMigrationPolicyEntity.cs` | Política de migração quando um modelo de protocolo é atualizado |

**Ged** — Gestão Eletrônica de Documentos

| Arquivo | Descrição |
|---|---|
| `GedDocumentEntity.cs` | Entidade principal de um documento: metadados, status, tenant, classe |
| `GedDocumentVersionEntity.cs` | Versão específica de um documento, permitindo histórico de alterações |
| `GedDocumentClassProfileEntity.cs` | Perfil de classificação do documento (tipo, categoria, regras) |
| `GedDocumentRetentionPolicyEntity.cs` | Política de retenção e descarte do documento |
| `GedDocumentSignatureEntity.cs` | Assinatura digital associada a uma versão do documento |
| `GedDocumentAuditEntryEntity.cs` | Entrada de auditoria: quem fez o quê e quando no documento |
| `GedDocumentProcessingJobEntity.cs` | Job assíncrono de processamento (OCR, conversão, indexação) |
| `GedDocumentStatuses.cs` | Enumeração dos estados do ciclo de vida de um documento GED |

**IA** — Inteligência Artificial e aprendizado de máquina

| Arquivo | Descrição |
|---|---|
| `AiDecisionEntity.cs` | Registro de uma decisão tomada (ou sugerida) pela IA |
| `AiFeedbackEntity.cs` | Feedback humano sobre uma sugestão da IA, usado para retreinamento |
| `AiSolicitationCaseEntity.cs` | Caso de análise de uma solicitação pelo modelo de IA |
| `SolicitationAiAnalysisModels.cs` | Modelos de dados para o resultado de análise de IA sobre solicitações |

**Notifications** — Notificações internas para usuários

**Organizations** — Setores e departamentos da organização

**Permissions** — Controle de acesso baseado em papéis (RBAC)

| Arquivo | Descrição |
|---|---|
| `RoleEntity.cs` | Papel/perfil de acesso (ex: Administrador, Analista) |
| `PermissionEntity.cs` | Permissão individual associada a um endpoint ou recurso |
| `RolePermissionEntity.cs` | Associação entre papel e permissão |
| `ApiKeyEntity.cs` | Chave de API para autenticação de sistemas integrados |
| `RefreshTokenEntity.cs` | Token de renovação de sessão JWT |
| `TokenBlacklistEntity.cs` | Lista de tokens invalidados (logout, expiração forçada) |

**Process** — Núcleo dos processos e solicitações

| Arquivo | Descrição |
|---|---|
| `ProtocolEntity.cs` | Agregado principal de um protocolo. Mantém status, histórico de eventos e referência ao processo BPMN |
| `ProtocolEventEntity.cs` | Registro imutável de um evento ocorrido no protocolo |
| `ProtocolIntegrationOutboxEntity.cs` | Mensagem de integração pendente de dispatch (padrão Outbox) |
| `SolicitationEntity.cs` | Solicitação inicial que, quando aprovada, origina um protocolo |
| `SolicitationProtocolTypeEntity.cs` | Tipo/categoria de protocolo associado a uma solicitação |

**RuleEngine** — Motor de regras de negócio

| Arquivo | Descrição |
|---|---|
| `RuleEngineEntities.cs` | Entidades que representam regras configuráveis avaliadas em tempo de execução |
| `RuleEngineDtos.cs` | DTOs para troca de dados com o motor de regras |
| `Contracts/RuleEngineModelPayload.cs` | Contrato do payload enviado ao motor de regras para avaliação |

**Tenants** — Multi-tenancy

| Arquivo | Descrição |
|---|---|
| `TenantBrandingEntity.cs` | Configurações de identidade visual por tenant (logo, cores, domínio) |

**Users** — Entidades de usuário

**Others** — Auditoria transversal

| Arquivo | Descrição |
|---|---|
| `AuditLogEntity.cs` | Log de auditoria funcional: operações de negócio relevantes |
| `ApiRequestAuditEntity.cs` | Log de auditoria de requisições HTTP recebidas pela API |

**Entidades base**

| Arquivo | Descrição |
|---|---|
| `BaseEntity.cs` | Classe base para todas as entidades. Contém campos comuns como `Id`, datas de criação/atualização |
| `ObjectCreateDelete.cs` | Mixin com metadados de ciclo de vida: criado por, removido por, datas |

#### Pasta `Interfaces/Repository/`

Contratos de repositório seguindo o padrão Repository. Cada interface define as operações de persistência disponíveis para uma entidade. As implementações ficam em `Acme.Sistemas.Repository`.

---

### Acme.Sistemas.Infrastructure

Camada de infraestrutura. Gerencia toda a comunicação com recursos externos: banco de dados, cache, storage, mensageria, e-mail, feature flags. Documentação detalhada disponível no arquivo principal de arquitetura.

```
Acme.Sistemas.Infrastructure/
├── Databases/
│   ├── Configuration/
│   │   ├── CrudConfiguration.cs
│   │   ├── DatabaseMetrics.cs
│   │   ├── DataConfiguration.cs
│   │   ├── RetryOptions.cs
│   │   ├── RetryPolicy.cs
│   │   ├── SqlExecutor.cs
│   │   └── TransactionManager.cs
│   ├── Helper/
│   │   ├── EntityMapper.cs
│   │   └── TransientErrorDetector.cs
│   └── Migrations/
│       ├── Configuration/
│       │   ├── AlterTableBuilder.cs
│       │   ├── FluentMigrationBuilder.cs
│       │   ├── IMigration.cs
│       │   ├── MigrationRunner.cs
│       │   ├── MigrationTable.cs
│       │   ├── SequenceBuilder.cs
│       │   └── TableBuilder.cs
│       └── Consts/
│           └── ConstDatabase.cs
├── Cache/
│   └── CacheStore.cs
├── Ged/
│   ├── GedAwsS3StorageProvider.cs
│   ├── GedAzureBlobStorageProvider.cs
│   ├── GedLocalStorageProvider.cs
│   ├── GedDocumentContentProcessor.cs
│   ├── GedDocumentStorageProviderResolver.cs
│   └── GedStorageSupport.cs
├── Messaging/
│   ├── Email/
│   │   └── EmailQueueService.cs
│   └── RabbitMq/
│       ├── RabbitMqBus.cs
│       ├── RabbitMqDynamicConfigurator.cs
│       ├── RabbitMqOptions.cs
│       └── RateLimiterConfig.cs
├── AppConfiguration/
│   └── FeatureFlagService.cs
└── InfrastructureServiceCollectionExtensions.cs
```

---

### Acme.Sistemas.ExternalIntegration

Camada de integração com APIs e serviços externos. Implementa um sistema de proxy HTTP baseado em reflexão, permitindo adicionar novos clientes externos sem repetição de código boilerplate.

```
Acme.Sistemas.ExternalIntegration/
├── Clients/
│   └── ViaCep/
│       └── IViaCepExternalClient.cs
├── Helper/
│   ├── ApiResponse.cs
│   ├── IApiResponse.cs
│   └── IExternalApiClient.cs
├── Methods/
│   ├── HeaderAttribute.cs
│   ├── HttpDeleteAttribute.cs
│   ├── HttpGetAttribute.cs
│   ├── HttpPostAttribute.cs
│   └── HttpPutAttribute.cs
├── Proxys/
│   ├── HttpClientProxy.cs
│   └── HttpClientProxyFactory.cs
└── ExternalIntegrationDI.cs
```

#### Pasta `Clients/`

Contém as interfaces de clientes externos, uma subpasta por serviço.

| Arquivo | Descrição |
|---|---|
| `Clients/ViaCep/IViaCepExternalClient.cs` | Interface para consulta de endereços pelo CEP na API pública ViaCEP. Os métodos são anotados com os atributos HTTP para serem descobertos pelo proxy |

#### Pasta `Helper/`

| Arquivo | Descrição |
|---|---|
| `IApiResponse.cs` | Interface base para respostas de APIs externas |
| `ApiResponse.cs` | Wrapper genérico para respostas externas. Encapsula o resultado, status HTTP e mensagens de erro de forma padronizada |
| `IExternalApiClient.cs` | Interface marcadora base para todos os clientes externos. Usada pelo factory para descoberta via DI |

#### Pasta `Methods/`

Atributos customizados usados para decorar os métodos das interfaces de clientes externos. O `HttpClientProxy` usa reflexão sobre esses atributos para montar e enviar a requisição HTTP correta.

| Arquivo | Descrição |
|---|---|
| `HttpGetAttribute.cs` | Marca um método como requisição HTTP GET e define a rota relativa |
| `HttpPostAttribute.cs` | Marca um método como requisição HTTP POST |
| `HttpPutAttribute.cs` | Marca um método como requisição HTTP PUT |
| `HttpDeleteAttribute.cs` | Marca um método como requisição HTTP DELETE |
| `HeaderAttribute.cs` | Injeta um cabeçalho HTTP customizado na requisição |

#### Pasta `Proxys/`

| Arquivo | Descrição |
|---|---|
| `HttpClientProxy.cs` | Implementação dinâmica de proxy HTTP. Usa reflexão para inspecionar os atributos de método (`HttpGet`, `HttpPost`, etc.) e executar a chamada HTTP correspondente, eliminando a necessidade de implementações manuais por cliente |
| `HttpClientProxyFactory.cs` | Fábrica que cria instâncias do `HttpClientProxy` para uma determinada interface de cliente externo. Integra com o `IHttpClientFactory` do ASP.NET Core |

#### Arquivo Raiz

| Arquivo | Descrição |
|---|---|
| `ExternalIntegrationDI.cs` | Registra os clientes externos e o proxy no container de DI |

---

### Acme.Sistemas.Repository

Camada de acesso a dados. Implementa as interfaces de repositório definidas em `Acme.Sistemas.Domain` usando SQL puro via `IDataConfiguration`.

```
Acme.Sistemas.Repository/
├── Helper/
│   └── ConvertExtentions.cs
├── Repositories/
│   └── V{N}/
│       └── {Entidade}/
│           ├── {Entidade}Repository.cs
│           └── Query/
│               └── {Entidade}Query.cs
└── RepositoryServiceCollectionExtensions.cs
```

#### Pasta `Helper/`

| Arquivo | Descrição |
|---|---|
| `ConvertExtentions.cs` | Métodos de extensão para conversão de tipos em resultados de queries. Facilita o mapeamento de colunas retornadas pelo banco para propriedades das entidades |

#### Pasta `Repositories/V{N}/`

Repositórios organizados por versão e por entidade. Cada subpasta de entidade contém:

| Arquivo | Descrição |
|---|---|
| `{Entidade}Repository.cs` | Implementação concreta do repositório. Executa SQL via `IDataConfiguration`. Em entidades complexas, pode ser dividida em arquivos parciais (ex: `.Admin.cs`, `.Mappers.cs`, `.Support.cs`) |
| `Query/{Entidade}Query.cs` | Classe que centraliza as queries SQL da entidade. Separa a montagem de SQL da lógica de execução |

**Exemplo de Entidades cobertas** (exemplos presentes na solução):

| Módulo | Repositórios |
|---|---|
| Endereços | `AddressRepository` |
| Usuários | `UserRepository` |
| Papéis e permissões | `RoleRepository`, `RolePermissionRepository` |
| Setores | `SectorRepository` |
| Tenants | `TenantRepository` |
| Solicitações | `SolicitationRepository` |
| Protocolos | `ProtocolWorkflowRepository`, `ProtocolEventRepository`, `ProtocolIntegrationOutboxRepository` |
| BPMN | Repositórios de processo, execução e definição de fluxo |
| GED | `GedDocumentRepository` (com partials: Admin, Mappers, Support) |
| Catálogo | `CatalogRepository` |
| Notificações | `NotificationRepository` |
| Tokens | `RefreshTokenRepository`, `TokenBlacklistRepository` |
| Auditoria | `AuditLogRepository`, `ApiRequestAuditRepository` |
| API Keys | `ApiKeyRepository` |
| E-mails | `EmailRepository` |
| Motor de regras | `RuleEngineRepository` |
| Navegação | `NavigationRepository` |
| Arquivos | `FileRepository` |

#### Arquivo Raiz

| Arquivo | Descrição |
|---|---|
| `RepositoryServiceCollectionExtensions.cs` | Registra todos os repositórios no container de DI |

---

### AutoProcess.Web

Camada de interface com o usuário. Projeto frontend da solução, responsável por consumir a API e apresentar as funcionalidades ao usuário final.

```
AutoProcess.Web/
├── Program.cs
└── WeatherApiClient.cs
```

| Arquivo | Descrição |
|---|---|
| `Program.cs` | Ponto de entrada e configuração da aplicação frontend |
| `WeatherApiClient.cs` | Exemplo de cliente HTTP para consumo de API. Serve como referência para novos clientes a serem implementados |

---

## Projetos de Teste

---

### Acme.Sistemas.IntegrationTest

Projeto de testes de integração. Sobe a aplicação completa com infraestrutura real (MySQL + RabbitMQ via Docker) e executa cenários end-to-end.

```
Acme.Sistemas.IntegrationTest/
├── Config/
│   ├── IntegrationTestBase.cs
│   ├── IntegrationWebApplicationFactory.cs
│   ├── DockerEnvironment.cs
│   └── TestOcrService.cs
├── Fixture/
│   ├── Bpmn/
│   ├── Employees/
│   ├── Roles/
│   ├── Solicitations/
│   ├── Tenants/
│   └── Users/
└── Test/
    ├── Bpmn/
    ├── Catalog/
    ├── Documentation/
    ├── Employees/
    ├── Ged/
    ├── Hybrid/
    ├── Navigation/
    ├── Roles/
    └── Solicitations/
```

#### Pasta `Config/`

| Arquivo | Descrição |
|---|---|
| `IntegrationWebApplicationFactory.cs` | Fábrica que sobe a aplicação em ambiente de teste. Configura MySQL e RabbitMQ via Docker, executa todas as migrations e substitui serviços problemáticos (ex: OCR) por versões de teste |
| `IntegrationTestBase.cs` | Classe base para todos os testes de integração. Fornece cliente HTTP configurado e utilitários de setup/teardown |
| `DockerEnvironment.cs` | Gerencia o ciclo de vida dos containers Docker necessários para os testes (início, parada, health checks) |
| `TestOcrService.cs` | Implementação stub do `IOcrService`. Retorna dados fixos para que testes não dependam de serviço externo de OCR |

#### Pasta `Fixture/`

Builders e helpers para criação de dados de teste pré-configurados. Organizados por módulo.

| Subpasta | Descrição |
|---|---|
| `Bpmn/` | Dados de fluxos BPMN, processos e definições para testes de motor de workflow |
| `Employees/` | Dados de colaboradores para cenários de gestão de pessoas |
| `Roles/` | Papéis e permissões pré-configurados |
| `Solicitations/` | Solicitações de exemplo para testar o fluxo de criação e aprovação |
| `Tenants/` | Configurações de tenant para testes multi-tenant |
| `Users/` | Usuários com diferentes perfis para testes de autenticação e autorização |

#### Pasta `Test/`

Testes agrupados por módulo. Cada arquivo de teste cobre um conjunto de cenários funcionais.

| Subpasta | Cobertura |
|---|---|
| `Bpmn/` | Motor BPMN: 11+ classes de teste cobrindo avanço de processo, gateways, UserTask, ServiceTask e branching paralelo |
| `Catalog/` | CRUD de catálogo e paginação |
| `Documentation/` | Acesso aos endpoints de documentação OpenAPI |
| `Employees/` | Gestão de colaboradores |
| `Ged/` | Upload, versionamento e recuperação de documentos GED |
| `Hybrid/` | Testes de pool de conexão e concorrência de banco de dados |
| `Navigation/` | Menus de navegação dinâmicos |
| `Roles/` | CRUD de papéis e associação de permissões |
| `Solicitations/` | Fluxo completo de solicitação: criação, análise e conversão em protocolo |

---

### Acme.Sistemas.Services.UnitTest

Projeto de testes unitários. Testa handlers, behaviors, services e regras de negócio de forma isolada, usando mocks.

```
Acme.Sistemas.Services.UnitTest/
├── Fixture/
│   ├── AnalyzeSolicitationCommandHandlerFixture.cs
│   ├── BpmnEngineFixture.cs
│   ├── BpmnIoParserFixture.cs
│   ├── CancelSolicitationCommandHandlerFixture.cs
│   ├── CatalogPaginationServiceFixture.cs
│   ├── CreateProtocolFromSolicitationCommandHandlerFixture.cs
│   ├── CreateSectorCommandHandlerFixture.cs
│   ├── CreateSolicitationCommandHandlerFixture.cs
│   ├── CreateUserCommandBehaviorFixture.cs
│   ├── CreateUserCommandHandlerFixture.cs
│   ├── EmployeeCommandHandlerFixture.cs
│   ├── GedDocumentHandlersFixture.cs
│   ├── GetEmployeeByIdQueryFixture.cs
│   ├── LoginQueryHandlerFixture.cs
│   ├── NavigationMenuQueryFixture.cs
│   ├── NodeRouterFixture.cs
│   ├── NotificationFixture.cs
│   ├── PasswordRecoveryServiceFixture.cs
│   ├── ProtocolAdministrationFixture.cs
│   ├── ProtocolExpirationEvaluatorFixture.cs
│   ├── ProtocolExpirationProcessorFixture.cs
│   ├── ProtocolModelCommandHandlerFixture.cs
│   ├── ProtocolModelPermissionsMatrixFixture.cs
│   ├── ProtocolModelQueryHandlerFixture.cs
│   ├── ProtocolWorkflowServiceFixture.cs
│   ├── RegisterAddressCommandHandlerFixture.cs
│   ├── RoleCommandHandlerFixture.cs
│   ├── RuleEngineExpressionFixture.cs
│   ├── RuleEngineServiceFixture.cs
│   ├── RuleModelSnapshotAdapterFixture.cs
│   ├── SearchAddressQueryHandlerFixture.cs
│   ├── TokenServiceFixture.cs
│   ├── UpdateSolicitationCommandHandlerFixture.cs
│   └── UserAccessScopeServiceFixture.cs
└── Test/
    ├── AnalyzeSolicitationCommandHandlerTests.cs
    ├── BpmnEngineTests.cs
    ├── BpmnIoParserTests.cs
    ├── CancelSolicitationCommandHandlerTests.cs
    ├── CatalogPaginationServiceTests.cs
    ├── CreateProtocolFromSolicitationCommandHandlerTests.cs
    ├── CreateSectorCommandHandlerTests.cs
    ├── CreateSolicitationCommandHandlerTests.cs
    ├── CreateUserCommandBehaviorTests.cs
    ├── CreateUserCommandHandlerTests.cs
    ├── EmployeeCommandHandlerTests.cs
    ├── GedDocumentHandlersTests.cs
    ├── GedModuleServiceTests.cs
    ├── GetEmployeeByIdQueryTests.cs
    ├── LoginQueryHandlerTests.cs
    ├── NavigationMenuQueryTests.cs
    ├── NodeRouterTests.cs
    ├── NotificationHandlersTests.cs
    ├── PasswordRecoveryServiceTests.cs
    ├── ProtocolAdministrationTests.cs
    ├── ProtocolExpirationEvaluatorTests.cs
    ├── ProtocolExpirationProcessorTests.cs
    ├── ProtocolModelCommandHandlerTests.cs
    ├── ProtocolModelPermissionsMatrixTests.cs
    ├── ProtocolModelQueryHandlerTests.cs
    ├── ProtocolRequestedInformationFlowTests.cs
    ├── ProtocolRequestedInformationSupportTests.cs
    ├── ProtocolSupportingServicesTests.cs
    ├── ProtocolWorkflowServiceTests.cs
    ├── RegisterAddressCommandHandlerTests.cs
    ├── RoleCommandHandlerTests.cs
    ├── RuleEngineExpressionEvaluatorTests.cs
    ├── RuleModelSnapshotAdapterTests.cs
    ├── SearchAddressQueryHandlerTests.cs
    ├── TokenServiceTests.cs
    ├── UpdateSolicitationCommandHandlerTests.cs
    └── UserAccessScopeServiceTests.cs
```

#### Pasta `Fixture/`

Cada fixture é uma classe de suporte que configura mocks, stubs e dados de entrada para os testes do handler/service correspondente. Utiliza as bibliotecas `Moq`, `Bogus` (geração de dados falsos) e `AutoMock`.

| Padrão de arquivo | Descrição |
|---|---|
| `{Nome}Fixture.cs` | Configura dependências mockadas, dados de entrada válidos e inválidos, e cenários de borda para o handler ou service nomeado |

#### Pasta `Test/`

Cada arquivo de teste corresponde a um handler, behavior ou service. Utiliza `xUnit` como framework de teste.

| Área | Testes presentes |
|---|---|
| **Usuários** | Criação, behavior de criação, login, tokens, escopo de acesso |
| **Solicitações** | Criação, atualização, cancelamento, análise por IA |
| **Protocolos** | Criação a partir de solicitação, administração, workflow, expiração, solicitação de informações |
| **BPMN** | Motor de execução, parser de BPMN IO, roteamento de nós |
| **GED** | Handlers de documentos, serviço do módulo GED |
| **Colaboradores** | Commands e queries de gestão de colaboradores |
| **Setores** | Criação de setores |
| **Papéis** | Commands de gerenciamento de papéis |
| **Catálogo** | Paginação do serviço de catálogo |
| **Endereços** | Registro e busca de endereços |
| **Navegação** | Query de menus de navegação |
| **Motor de Regras** | Avaliador de expressões, adaptador de snapshot, serviço de regras |
| **Recuperação de senha** | Fluxo de recuperação via e-mail |
| **Notificações** | Handlers de notificações internas |
| **Modelos de protocolo** | Commands, queries e matriz de permissões |

---

## Padrões Arquiteturais da Plataforma

| Padrão | Onde é aplicado |
|---|---|
| **CQRS + Mediator** | `Acme.Sistemas.Core` (contrato) + `Acme.Sistemas.Services` (implementação) |
| **Pipeline Behavior** | `IPipelineBehavior` no Core, implementado nos `*Behavior.cs` de cada funcionalidade |
| **Repository Pattern** | Interfaces em `Acme.Sistemas.Domain`, implementações em `Acme.Sistemas.Repository` |
| **Outbox Pattern** | `ProtocolIntegrationOutboxEntity` + worker de dispatch |
| **Domain-Driven Design** | Entidades ricas em `Acme.Sistemas.Domain`, agregados, value objects |
| **BPMN Workflow Engine** | `BpmnEngine` em `Acme.Sistemas.Services`, entidades em `Domain`, dados em `Repository` |
| **Multi-tenancy** | Isolamento por tenant em todas as entidades e repositórios |
| **Proxy HTTP por Reflexão** | `HttpClientProxy` em `Acme.Sistemas.ExternalIntegration` |
| **Migration Versionada** | `MigrationRunner` + `IMigration` + tabela `__Migrations` |
| **Feature Flags** | `featureflags.json` + `FeatureFlagService` com hot-reload |
| **Auditoria Funcional** | `AuditLogEntity` + middleware de auditoria de requests |

---

## Convenções de Testes

Todo método `[Fact]` ou `[Theory]` (incluindo `[Fact(Skip = "...")]`) nos projetos `Acme.Sistemas.Services.UnitTest` e `Acme.Sistemas.IntegrationTest` deve declarar três attributes:

```csharp
[Trait("Solucao", "Services")]
[Trait("Acao", "CriarDespesa")]
[Fact(DisplayName = "Dado dados válidos, quando criar despesa, então persiste e retorna 201")]
public async Task CriarDespesa_DadosValidos_Retorna201()
{
    // ...
}
```

### `Trait("Solucao", X)` — vocabulário fechado

| Valor | Quando usar |
|---|---|
| `Api` | Endpoints, integração HTTP, middlewares, host |
| `Services` | Handlers de Command/Query/Notification, behaviors do pipeline |
| `Core` | Helpers e utilitários puros (`Acme.Sistemas.Core`) — Jwt, Hash, Password |
| `Domain` | Entidades, value objects, regras de invariante |
| `Repository` | Repositórios SQL, filtro de tenant |
| `Infrastructure` | Cache, mensageria, email, GED, hosted services |
| `ExternalIntegration` | `HttpClientProxy`, ViaCEP, integrações externas |
| `Test` | Meta-tests (convenções, layout, blueprint) |

### `Trait("Acao", Y)` — nome curto da unidade-em-teste

| Tipo | Exemplo |
|---|---|
| Command | `CriarDespesa`, `Login`, `BaixarDespesa` |
| Query | `ListarLogs`, `ObterFluxo`, `GerarBalanco` |
| Behavior | `AuditBehavior`, `CacheLookupBehavior`, `LogBehavior`, `ValidationBehavior` |
| Service / Helper | `JwtTokenService`, `PasswordHelper`, `FeatureFlagService`, `HybridCacheStore` |
| Worker / Hosted | `CacheCleanupWorker` |
| Repository / Filtro | `TenantFilter` |
| Aspecto de Api | `HealthCheck`, `RouteSnapshot`, `IsolamentoCrossTenant`, `TenantContext`, `FluxoVenda` |
| Meta / convenções | `Convencoes` |

### `DisplayName` — Given-When-Then em PT-BR

Forma canônica: `"Dado <contexto>, quando <ato>, então <resultado>"`.

Variações aceitas:
- Omitir "Dado <contexto>" se o contexto é trivial: `"Quando login com senha errada, então retorna 401"`.
- Usar "Deve <comportamento>" como prefixo se o cenário é estado-livre: `"Deve gerar hash diferente para mesma senha em chamadas distintas"`.

### Filtros úteis no `dotnet test`

```powershell
# Roda apenas a camada Services
dotnet test --filter "Trait=Solucao=Services"

# Roda todos os testes da unidade CriarDespesa (handler + endpoint)
dotnet test --filter "Trait=Acao=CriarDespesa"
```

### Enforcement

O método `TodoTeste_TemDisplayNameESolucaoEAcao` em `ConvencoesBlueprintTests` (projeto Unit) reprova qualquer regressão. Falta de DisplayName, `Trait("Solucao")` fora da allow-list ou `Trait("Acao")` vazio quebra o build.
