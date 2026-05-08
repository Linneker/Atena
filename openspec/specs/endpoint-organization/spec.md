# endpoint-organization Specification

## Purpose
Organização física dos endpoints HTTP da API: cada rota mora em pasta dedicada com 4 arquivos (Endpoint+Request+Response+Map), uma rota por classe `IEndpoint`. Garante coesão local e reviewability — pull requests tocam pastas pequenas, não monoliths.

## Requirements

### Requirement: Cada rota HTTP em pasta dedicada com 4 arquivos
O sistema SHALL organizar cada rota HTTP em uma pasta `Endpoints/V1/{Recurso}/{Verbo}{Recurso}/` contendo os 4 arquivos `{Verbo}{Recurso}Endpoint.cs`, `{Verbo}{Recurso}Request.cs`, `{Verbo}{Recurso}Response.cs` e `{Verbo}{Recurso}Map.cs`.

#### Scenario: Endpoint isolado em sua própria pasta
- **WHEN** uma nova rota HTTP é registrada
- **THEN** existe uma pasta `Endpoints/V1/{Recurso}/{Verbo}{Recurso}/` com 4 arquivos: `{Verbo}{Recurso}Endpoint.cs`, `{Verbo}{Recurso}Request.cs`, `{Verbo}{Recurso}Response.cs`, `{Verbo}{Recurso}Map.cs`
- **THEN** a classe `IEndpoint` registra apenas essa rota (não outras)

#### Scenario: Sem aglomerados multi-rota
- **WHEN** o codebase é inspecionado
- **THEN** não existem arquivos `*Endpoints.cs` (plural) em `Endpoints/V1/`
- **THEN** cada `IEndpoint` registra exatamente uma rota HTTP em seu `MapEndpoint`

### Requirement: Verificação de conformidade automatizada
O sistema SHALL falhar a suíte de testes de CI quando uma rota HTTP é registrada fora do padrão 4-arquivos.

#### Scenario: Test analyzer enumera rotas runtime
- **WHEN** `EndpointConventionTests` (projeto de integração) executa em CI
- **THEN** o test enumera todas as rotas via `EndpointDataSource.Endpoints` em runtime
- **THEN** para cada rota `/api/v1/*`, resolve a classe `IEndpoint` declarante via `MethodInfo` metadata e verifica que reside em pasta com siblings `*Request.cs`, `*Response.cs` e `*Map.cs`
- **THEN** falha qualquer rota órfã; allow-list cobre apenas `/health`

#### Scenario: Snapshot de rotas garante zero regressão
- **WHEN** o codebase é modificado em refactors envolvendo endpoints
- **THEN** existe um baseline `test/Integration/Acme.Sistemas.IntegrationTest/Baseline/routes-runtime.json`
- **THEN** o test `RouteSnapshotTests.RotasEnumeradas_BatemComBaseline` falha se qualquer path/verb/name divergir do baseline
