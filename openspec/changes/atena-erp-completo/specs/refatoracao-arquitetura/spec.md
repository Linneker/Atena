## ADDED Requirements

### Requirement: Estrutura de Projetos Acme Blueprint
O sistema SHALL adotar a estrutura de projetos definida no blueprint.yml — eliminando os projetos `acme.atena.*` e criando os projetos padrão Acme.

#### Scenario: Estrutura de pastas após migração
- **WHEN** a migração for concluída
- **THEN** a solução SHALL conter os projetos: `Acme.Sistemas.Atena.Api`, `Acme.Sistemas.Services`, `Acme.Sistemas.Core`, `Acme.Sistemas.Domain`, `Acme.Sistemas.Repository`, `Acme.Sistemas.Infrastructure`, `Acme.Sistemas.ExternalIntegration`
- **THEN** os projetos de teste SHALL estar em `test/Integration/` e `test/Unit/`

#### Scenario: Prefixo e nomenclatura padrão
- **WHEN** qualquer novo projeto ou arquivo for criado
- **THEN** seguirá o prefixo `Acme.Sistemas.Atena` para projetos e namespaces correspondentes

### Requirement: Minimal API com Endpoints Versionados
A camada de API SHALL usar Minimal API com endpoints organizados por versão (`V1`, `V2`) e por funcionalidade, eliminando os Controllers MVC atuais.

#### Scenario: Endpoint seguindo padrão Minimal API
- **WHEN** um novo endpoint é criado
- **THEN** SHALL existir um arquivo `NomeEndpoint.cs` implementando `IEndpoint`
- **THEN** SHALL existir `NomeEndpointRequest.cs` (se aplicável), `NomeEndpointResponse.cs` e `NomeEndpointMap.cs`
- **THEN** o endpoint SHALL ser registrado via extension method em `Program.cs`

#### Scenario: Rota versionada
- **WHEN** um endpoint é exposto
- **THEN** a rota SHALL seguir o padrão `/api/v1/{recurso}` para a versão inicial
- **THEN** versões anteriores continuam funcionando quando uma nova versão é adicionada

### Requirement: CQRS por Funcionalidade no Projeto Services
Toda lógica de negócio SHALL ser organizada por funcionalidade no projeto `Acme.Sistemas.Services`, com Command, Query e Event separados, cada um com Handler, Behavior, Result e Validation.

#### Scenario: Criação de novo Command
- **WHEN** uma nova ação de escrita é implementada (ex: CriarDespesa)
- **THEN** SHALL existir: `CriarDespesaCommand.cs`, `CriarDespesaCommandHandler.cs`, `CriarDespesaCommandBehavior.cs`, `CriarDespesaCommandResult.cs`, `CriarDespesaCommandValidation.cs`
- **THEN** toda a lógica de negócio reside no Handler; validações estruturais no Validation; logs e enriquecimento no Behavior

#### Scenario: AutoMapper removido
- **WHEN** a migração for concluída
- **THEN** não SHALL existir referência ao pacote `AutoMapper` em nenhum projeto
- **THEN** o mapeamento SHALL ser feito em arquivos `*Map.cs` no projeto Api ou manualmente no Handler

### Requirement: Repositórios com SQL Puro
O acesso a dados SHALL usar SQL puro via `IDataConfiguration` no projeto Repository, removendo o uso de EF Core para queries (mantendo apenas para migrations).

#### Scenario: Repositório com query SQL
- **WHEN** um repositório é criado para uma entidade
- **THEN** as queries SQL ficam em arquivo separado `EntidadeQuery.cs` dentro de `Query/`
- **THEN** o repositório implementa a interface definida em `Acme.Sistemas.Domain/Interfaces/Repository/`

#### Scenario: EF Core apenas para migrations
- **WHEN** uma nova tabela ou coluna precisa ser criada
- **THEN** uma migration versionada é criada via `MigrationRunner` e `IMigration` conforme padrão da Infrastructure
- **THEN** o `DbContext` do EF Core é usado somente para migrations, não para queries de produção

### Requirement: Multi-tenancy no nível do Repository
Todos os repositórios SHALL filtrar automaticamente por `tenant_id` sem necessidade de o Handler informar explicitamente.

#### Scenario: Filtro automático de tenant
- **WHEN** qualquer método de repositório é chamado
- **THEN** o `tenant_id` é obtido do contexto da requisição (via IHttpContextAccessor ou similar)
- **THEN** todas as queries incluem `WHERE tenant_id = @tenantId` automaticamente
- **THEN** é impossível para um Handler recuperar dados de outro tenant acidentalmente

### Requirement: Organização por Versão no Services
As funcionalidades no projeto Services SHALL ser organizadas por versão (`V1`, `V2`) para permitir evolução sem quebra de contratos.

#### Scenario: Funcionalidade na versão 1
- **WHEN** a primeira implementação de uma funcionalidade é criada
- **THEN** ela reside em `V1/NomeFuncionalidade/Command|Query|Event/`

#### Scenario: Evolução para versão 2 sem quebrar V1
- **WHEN** uma funcionalidade precisa de mudança breaking
- **THEN** a nova implementação é criada em `V2/NomeFuncionalidade/`
- **THEN** o endpoint V1 continua funcionando apontando para os handlers V1
