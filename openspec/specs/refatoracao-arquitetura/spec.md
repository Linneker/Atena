# refatoracao-arquitetura Specification

## Purpose
TBD - created by archiving change aderencia-blueprint-acme. Update Purpose after archive.
## Requirements
### Requirement: Aderência ao layout do blueprint Acme
O sistema SHALL aderir ao layout físico definido em `documentacao/blueprint.yml` e `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md`, com a única adaptação documentada de manter o Domain organizado por módulo ERP (e não pelas categorias genéricas do AutoProcess), porque cada projeto da plataforma Acme é dono do seu próprio domínio.

#### Scenario: Estrutura de pastas raiz da solução
- **WHEN** um desenvolvedor inspeciona a raiz do repositório
- **THEN** existem `infra/compose/`, `infra/k8s/v1/`, `infra/k8s/kind-config.yaml`, `src/Api`, `src/Service`, `src/Data`, `test/Integration`, `test/Unit`
- **THEN** `docker-compose.yml` está em `infra/compose/`, não na raiz

#### Scenario: Realocação entre projetos completa
- **WHEN** um desenvolvedor inspeciona os projetos
- **THEN** `Core/Messaging` não existe (movido para `Infrastructure/Messaging`)
- **THEN** `Core/Reports` não existe (movido para `Services/V1/Relatorios`)
- **THEN** `Infrastructure/Hosted` não existe (movido para `Api/Hosted`)
- **THEN** `Repository/Configuration` não existe (movido para `Infrastructure/Databases/Configuration`)

### Requirement: CQRS quíntuplo obrigatório
O sistema SHALL exigir que cada Command e Query no projeto `Acme.Sistemas.Services` tenha exatamente 5 arquivos no padrão: `{Nome}Command.cs`, `{Nome}CommandHandler.cs`, `{Nome}CommandBehavior.cs`, `{Nome}CommandResult.cs`, `{Nome}CommandValidation.cs` (ou os equivalentes Query). Eventos têm 4 arquivos: Notification, Handler, Behavior, Validation (sem Result, pois eventos não retornam).

#### Scenario: Behavior implementa as 4 responsabilidades mínimas
- **WHEN** um desenvolvedor cria `CriarDespesaCommandBehavior.cs`
- **THEN** o arquivo implementa: log estruturado, consulta a cache (com retorno imediato em hit para queries), validações complementares de regra de negócio e enriquecimento de dados
- **THEN** o Behavior nunca é vazio nem é mero pass-through

#### Scenario: Result usa ResponseDefault
- **WHEN** um Handler retorna `CriarDespesaCommandResult`
- **THEN** o `Result` é convertido em `ResponseDefault<T>` no `Map.cs` do endpoint
- **THEN** falhas viram `ResponseDefault.Erro(...)` com lista de `Error { Code, Message }`

#### Scenario: Teste-analyzer bloqueia merge
- **WHEN** um desenvolvedor adiciona um Command sem Behavior ou sem Result
- **THEN** o teste `ConvencoesBlueprintTests` falha
- **THEN** a pipeline CI bloqueia o merge

### Requirement: Behaviors transversais registrados uma vez no Core
O sistema SHALL prover Behaviors transversais (`ValidationBehavior`, `LogBehavior`, `CacheLookupBehavior`, `AuditBehavior`) em `Acme.Sistemas.Core/Mediators/Behaviors/`, registrados no DI uma única vez, executando na ordem: Validation → CacheLookup → Audit → Log → Handler específico.

#### Scenario: Pipeline de execução padrão
- **WHEN** um Command é despachado pelo `Mediator.Send`
- **THEN** ValidationBehavior executa primeiro
- **THEN** CacheLookupBehavior executa em seguida (skip se Command, execute se Query com `ICacheable`)
- **THEN** AuditBehavior executa antes do handler para Commands com `IAuditable`
- **THEN** o Behavior específico da funcionalidade executa por último, antes do Handler

### Requirement: Endpoints organizados em pastas de 4 arquivos por verbo
O sistema SHALL organizar cada verbo HTTP em uma pasta dedicada com `{Verbo}{Recurso}.cs` (implementa `IEndpoint`), `{Verbo}{Recurso}Request.cs` (opcional para GETs simples), `{Verbo}{Recurso}Response.cs`, `{Verbo}{Recurso}Map.cs`. As rotas, payloads e contratos HTTP NÃO mudam em relação à versão atual — apenas a organização física dos arquivos.

#### Scenario: Reorganização de DespesaEndpoints
- **WHEN** um desenvolvedor inspeciona `Api/Endpoints/V1/Despesa/`
- **THEN** encontra subpastas `CriarDespesa/`, `AlterarDespesa/`, `ExcluirDespesa/`, `ListarDespesas/`, `ObterDespesa/`, `BaixarDespesa/`
- **THEN** cada subpasta tem os 4 arquivos do padrão
- **THEN** `DespesaEndpoints.cs` monolítico não existe mais

#### Scenario: GET simples pode omitir Request
- **WHEN** um endpoint é `GET /api/v1/despesas/{id}` sem query string
- **THEN** o `Request.cs` pode ser omitido
- **THEN** os outros 3 arquivos (Endpoint, Response, Map) continuam obrigatórios

#### Scenario: Rota mantida após split
- **WHEN** um cliente HTTP chama `POST /api/v1/despesas` antes e depois da reorganização
- **THEN** o payload de entrada e a resposta são idênticos
- **THEN** o status code é idêntico

### Requirement: Domain mantido por módulo ERP (adaptação documentada)
O sistema SHALL manter `Acme.Sistemas.Domain/Entities/` organizado por módulos ERP do Atena (Cadastros, Vendas, Financeiro, Estoque, Compras, Fiscal, Permissions, Tenants, Users, Auditoria, Produtos), em vez das categorias genéricas listadas no blueprint (Archives, Bpmn, Process, Flows, Ged, IA, RuleEngine, etc.) que pertencem ao AutoProcess.

#### Scenario: Justificativa documentada
- **WHEN** um desenvolvedor consulta `CLAUDE.md` ou o `proposal.md` desta change
- **THEN** encontra a justificativa: cada projeto Acme é dono do seu Domain; categorias do blueprint são exemplos do AutoProcess
- **THEN** a aderência ao blueprint é técnica (CQRS, layout de pastas, infra), não de modelo de domínio

#### Scenario: Convenções técnicas continuam aplicadas
- **WHEN** um desenvolvedor cria uma nova entidade ERP no Atena
- **THEN** ela herda `BaseEntity`, fica em uma subpasta de módulo ERP em `Domain/Entities/`
- **THEN** sua interface de repositório fica em `Domain/Interfaces/Repository/`
- **THEN** sua implementação SQL fica em `Repository/Repositories/V1/{Entidade}/`
- **THEN** suas operações ficam em `Services/V1/{Funcionalidade}/Command|Query/...` com o quinteto completo

### Requirement: Workers em Api/Hosted
O sistema SHALL hospedar todos os `IHostedService` em `src/Api/Acme.Sistemas.Atena.Api/Hosted/`, conforme blueprint, em vez de em `Infrastructure/Hosted`.

#### Scenario: NFeTransmissaoWorker localizado em Api
- **WHEN** um desenvolvedor procura o worker de NF-e
- **THEN** encontra em `Api/Hosted/NFeTransmissaoWorker.cs`
- **THEN** o registro `AddHostedService<NFeTransmissaoWorker>()` está no `Program.cs`

#### Scenario: CacheCleanupWorker em Api/Hosted
- **WHEN** o worker de eviction de cache executa
- **THEN** sua classe está em `Api/Hosted/CacheCleanupWorker.cs`
- **THEN** depende de `ICacheStore` injetado por DI

