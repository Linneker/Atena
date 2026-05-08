## ADDED Requirements

### Requirement: Cache híbrido em camadas
O sistema SHALL prover cache distribuído gratuito por padrão usando LiteDB single-file (cold layer) com `IMemoryCache` em cima (hot layer), com Redis como provider opcional ativável por feature flag.

#### Scenario: Hit na camada quente
- **WHEN** um caller solicita uma chave existente em `IMemoryCache`
- **THEN** o sistema retorna o valor em <1ms sem consultar LiteDB

#### Scenario: Miss em quente, hit em fria
- **WHEN** uma chave não está em `IMemoryCache` mas existe em `cache.db`
- **THEN** o sistema lê do LiteDB, popula a camada quente e retorna o valor

#### Scenario: Miss em ambas
- **WHEN** a chave não existe em nenhuma camada
- **THEN** o `CacheLookupBehavior` continua o pipeline executando o handler real
- **THEN** o resultado é gravado em ambas as camadas com o TTL definido

### Requirement: TTL padrão de 15 minutos
O sistema SHALL aplicar TTL absoluto padrão de 15 minutos para entradas de cache, com possibilidade de override por chamador via `ICacheable.Ttl`.

#### Scenario: Expiração após 15 minutos
- **WHEN** uma entrada é gravada sem TTL explícito
- **THEN** após 15 minutos da gravação, a próxima leitura retorna miss

#### Scenario: TTL customizado via ICacheable
- **WHEN** uma `Query` implementa `ICacheable` com `Ttl = TimeSpan.FromHours(1)`
- **THEN** o resultado em cache expira em 1 hora, não 15 minutos

### Requirement: Provider configurável via feature flag
O sistema SHALL permitir alternar o provider de cache entre `LiteDb` (default) e `Redis` via flag `Cache:Provider`, com efeito imediato após reload da flag.

#### Scenario: Default usa LiteDB
- **WHEN** a flag `Cache:Provider` não está definida ou tem valor `LiteDb`
- **THEN** o `CacheStore` resolvido pela DI é a implementação LiteDB+Memory

#### Scenario: Alternância para Redis em runtime
- **WHEN** a flag muda para `Redis` via `PUT /api/v1/feature-flags/Cache:Provider`
- **THEN** o sistema passa a usar `RedisCacheStore` nas próximas chamadas
- **THEN** o `cache.db` continua existindo localmente como fallback de bootstrap

#### Scenario: Conexão Redis indisponível
- **WHEN** a flag está em `Redis` mas o servidor Redis está offline
- **THEN** o sistema cai automaticamente em `LiteDb` para a chamada corrente
- **THEN** um log de warning é emitido

### Requirement: Localização do arquivo cache.db
O sistema SHALL armazenar `cache.db` no diretório raiz do `Api` em desenvolvimento e em `/tmp/cache.db` em containers Kubernetes (per-pod, ephemeral, sem PVC).

#### Scenario: Path em desenvolvimento
- **WHEN** o serviço inicia localmente
- **THEN** o LiteDB grava em `src/Api/Acme.Sistemas.Atena.Api/cache.db`

#### Scenario: Path em K8s
- **WHEN** o pod inicia em cluster K8s
- **THEN** o LiteDB grava em `/tmp/cache.db`
- **THEN** o caminho é montado como volume `emptyDir` no `deployment.yaml`

#### Scenario: Restart de pod limpa cache local
- **WHEN** um pod é reiniciado
- **THEN** seu `/tmp/cache.db` é descartado
- **THEN** o cache é repopulado conforme as próximas leituras
- **THEN** o comportamento é idêntico ao AutoProcess

### Requirement: Eviction periódico de entradas expiradas
O sistema SHALL executar um background worker (`CacheCleanupWorker`) a cada 5 minutos que remove do `cache.db` todas as entradas com TTL vencido.

#### Scenario: Limpeza periódica
- **WHEN** o worker executa
- **THEN** entradas com `expiresAt < DateTime.UtcNow` são deletadas do LiteDB
- **THEN** o tamanho do arquivo é compactado se exceder 50% de espaço morto

#### Scenario: Limite soft de tamanho
- **WHEN** o `cache.db` excede 10 GB
- **THEN** o worker emite log de warning
- **THEN** o worker remove as 20% entradas mais antigas para reduzir o tamanho

### Requirement: Concorrência segura intra-pod
O sistema SHALL suportar leitura e escrita concorrente do `cache.db` por múltiplas threads do mesmo processo sem corrupção, usando o modo `Connection=shared` do LiteDB.

#### Scenario: 10 threads simultâneas
- **WHEN** 10 threads escrevem 1.000 entradas distintas no mesmo `cache.db`
- **THEN** todas as gravações persistem
- **THEN** nenhum erro de I/O concorrente é lançado

### Requirement: CacheStore como contrato único
O sistema SHALL expor uma única interface `ICacheStore` em `Acme.Sistemas.Domain/Interfaces/`, consumida por `CacheLookupBehavior` e por código de aplicação que precise de cache explícito.

#### Scenario: Substituição transparente do provider
- **WHEN** o provider muda de LiteDb para Redis
- **THEN** consumidores de `ICacheStore` não precisam ser alterados
- **THEN** apenas o registro no DI muda
