## ADDED Requirements

### Requirement: Cada rota HTTP em pasta dedicada com 4 arquivos
O sistema SHALL organizar cada rota HTTP em uma pasta `Endpoints/V1/{Recurso}/{Verbo}{Recurso}/` contendo os arquivos `{Verbo}{Recurso}Endpoint.cs`, `{Verbo}{Recurso}Response.cs`, `{Verbo}{Recurso}Map.cs`, e opcionalmente `{Verbo}{Recurso}Request.cs`.

#### Scenario: Endpoint isolado em sua própria pasta
- **WHEN** uma nova rota HTTP é registrada
- **THEN** existe uma pasta `Endpoints/V1/{Recurso}/{Verbo}{Recurso}/` com pelo menos 3 arquivos: `{Verbo}{Recurso}Endpoint.cs`, `{Verbo}{Recurso}Response.cs`, `{Verbo}{Recurso}Map.cs`
- **THEN** a classe `IEndpoint` registra apenas essa rota (não outras)

#### Scenario: Request opcional em GETs simples
- **WHEN** a rota é GET sem body de entrada (apenas path params + query string)
- **THEN** o arquivo `{Verbo}{Recurso}Request.cs` pode ser omitido
- **THEN** o mapping de parâmetros HTTP → Command/Query é feito inline no Endpoint

#### Scenario: Sem aglomerados multi-rota
- **WHEN** o codebase é inspecionado
- **THEN** não existem arquivos `*Endpoints.cs` (plural) em `Endpoints/V1/`
- **THEN** cada `IEndpoint` registra exatamente uma rota HTTP em seu `MapEndpoint`

### Requirement: Verificação de conformidade automatizada
O sistema SHALL falhar a compilação ou os testes de CI quando uma rota HTTP é registrada fora do padrão 4-arquivos.

#### Scenario: Test analyzer enumera rotas runtime
- **WHEN** `ConvencoesBlueprintTests` executa em CI
- **THEN** o test enumera todas as rotas via `EndpointDataSource.Endpoints` em runtime
- **THEN** para cada rota, verifica que o arquivo `IEndpoint` registrante reside em pasta com siblings `Response.cs` e `Map.cs`
- **THEN** falha qualquer rota órfã

#### Scenario: Snapshot de rotas garante zero regressão durante refactor
- **WHEN** o split de monoliths está em andamento
- **THEN** existe um baseline `routes-runtime.json` capturado antes do split
- **THEN** o test `RotasEnumeradas_BatemComBaseline` falha se qualquer path/verb/name mudou
