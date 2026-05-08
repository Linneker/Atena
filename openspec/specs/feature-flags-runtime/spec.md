# feature-flags-runtime Specification

## Purpose
TBD - created by archiving change aderencia-blueprint-acme. Update Purpose after archive.
## Requirements
### Requirement: Feature flags com hot-reload
O sistema SHALL carregar feature flags do arquivo `featureflags.json` no diretório raiz do Api e recarregá-las automaticamente quando o arquivo for alterado em disco, sem reinício do processo.

#### Scenario: Alteração no arquivo provoca reload em runtime
- **WHEN** o arquivo `featureflags.json` é alterado em disco
- **THEN** o `IOptionsMonitor<FeatureFlagSettings>` notifica os consumidores em até 5 segundos
- **THEN** chamadas subsequentes a serviços que consultam a flag observam o novo valor

#### Scenario: Arquivo malformado não derruba o serviço
- **WHEN** o arquivo `featureflags.json` é gravado com JSON inválido
- **THEN** o sistema mantém os valores anteriores em memória
- **THEN** um log de erro é emitido com identificação clara do erro de parse

### Requirement: Endpoint de listagem de feature flags
O sistema SHALL expor `GET /api/v1/feature-flags` que retorna todas as feature flags ativas com seus valores atuais, protegido pela permissão `feature-flags.read`.

#### Scenario: Listagem por usuário autorizado
- **WHEN** um usuário com permissão `feature-flags.read` faz `GET /api/v1/feature-flags`
- **THEN** o sistema retorna HTTP 200 com array de `{ key, value, type }`

#### Scenario: Listagem por usuário não autorizado
- **WHEN** um usuário sem a permissão faz a chamada
- **THEN** o sistema retorna HTTP 403

### Requirement: Endpoint de obtenção de feature flag específica
O sistema SHALL expor `GET /api/v1/feature-flags/{key}` retornando o valor atual de uma flag, protegido por `feature-flags.read`.

#### Scenario: Flag existente
- **WHEN** a flag `Cache:Provider` existe
- **THEN** o endpoint retorna HTTP 200 com `{ key: "Cache:Provider", value: "LiteDb" }`

#### Scenario: Flag inexistente
- **WHEN** o caller solicita uma flag não cadastrada
- **THEN** o sistema retorna HTTP 404

### Requirement: Endpoint de alteração de feature flag
O sistema SHALL expor `PUT /api/v1/feature-flags/{key}` que persiste o novo valor em `featureflags.json`, protegido por `feature-flags.write`.

#### Scenario: Alteração persiste no arquivo
- **WHEN** um admin envia `PUT /api/v1/feature-flags/Cache:Provider` com body `{ value: "Redis" }`
- **THEN** o sistema grava o novo valor em `featureflags.json`
- **THEN** o `IOptionsMonitor` propaga a mudança em até 5 segundos
- **THEN** chamadas subsequentes ao `CacheStore` usam o provider Redis

#### Scenario: Tipo inválido
- **WHEN** o caller envia um valor de tipo incompatível (ex: string em flag boolean)
- **THEN** o sistema retorna HTTP 400 com mensagem clara
- **THEN** o arquivo não é alterado

### Requirement: Endpoint de recarga forçada
O sistema SHALL expor `POST /api/v1/feature-flags/recarregar` que força a releitura do `featureflags.json` mesmo sem alteração de disco, protegido por `feature-flags.write`.

#### Scenario: Recarga manual
- **WHEN** um admin faz `POST /api/v1/feature-flags/recarregar`
- **THEN** o sistema reabre o arquivo, valida o JSON e atualiza os valores em memória
- **THEN** retorna HTTP 200 com timestamp da recarga

### Requirement: Endpoints organizados no padrão 4-arquivos
Os endpoints de feature flags SHALL seguir o layout do blueprint: para cada verbo HTTP, uma pasta com `{Verbo}{Recurso}.cs`, `Request.cs` (quando aplicável), `Response.cs`, `Map.cs`.

#### Scenario: Estrutura de pasta para alteração de flag
- **WHEN** um desenvolvedor inspeciona `Api/Endpoints/V1/FeatureFlags/AlterarFeatureFlag/`
- **THEN** encontra `AlterarFeatureFlag.cs`, `AlterarFeatureFlagRequest.cs`, `AlterarFeatureFlagResponse.cs`, `AlterarFeatureFlagMap.cs`

