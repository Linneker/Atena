# Design — Aderência ao Blueprint Acme

## Princípios

1. **Blueprint = norma técnica, não modelo de domínio.** Cada projeto Acme tem o seu Domain. O Atena é ERP; AutoProcess é BPMN. Eles compartilham convenções (CQRS, layout, infra, cache), não entidades.
2. **Sem breaking changes na API.** Rotas, payloads, status codes não mudam. A reorganização é interna.
3. **Behaviors são regra, não exceção.** Todo Command/Query tem Behavior. Vazio é proibido — no mínimo log + cache lookup + validação complementar.
4. **Cache distribuído gratuito por padrão.** Redis é opt-in.

---

## 1. Layout final

```
H:\ACME\PROJETOS\Sistemas\Atena\
├── infra/
│   ├── compose/
│   │   └── docker-compose.yml         ← movido da raiz
│   └── k8s/
│       ├── v1/
│       │   ├── deployment.yaml
│       │   ├── configmap.yaml
│       │   ├── service.yaml
│       │   └── ingress.yaml
│       └── kind-config.yaml           ← novo (3 control-plane + 3 worker como AutoProcess)
├── src/
│   ├── Api/
│   │   └── Acme.Sistemas.Atena.Api/
│   │       ├── Endpoints/V1/{Recurso}/{Verbo}{Recurso}/
│   │       │   ├── {Verbo}{Recurso}.cs
│   │       │   ├── {Verbo}{Recurso}Request.cs    (opcional)
│   │       │   ├── {Verbo}{Recurso}Response.cs
│   │       │   └── {Verbo}{Recurso}Map.cs
│   │       ├── Middlewares/
│   │       ├── Hosted/                ← movido de Infrastructure/Hosted
│   │       ├── Config/
│   │       ├── cache.db               ← LiteDB local (gitignored)
│   │       ├── featureflags.json
│   │       ├── Dockerfile
│   │       └── Program.cs
│   ├── Service/
│   │   ├── Acme.Sistemas.Services/V1/{Funcionalidade}/
│   │   │   ├── Command/{Acao}/
│   │   │   │   ├── {Acao}Command.cs
│   │   │   │   ├── {Acao}CommandHandler.cs
│   │   │   │   ├── {Acao}CommandBehavior.cs    ← NOVO
│   │   │   │   ├── {Acao}CommandResult.cs      ← NOVO
│   │   │   │   └── {Acao}CommandValidation.cs
│   │   │   ├── Query/{Acao}/  (mesmo quinteto)
│   │   │   ├── Event/{Acao}/  (Notification + Handler + Behavior + Validation)
│   │   │   └── Services/
│   │   │       ├── I{Nome}Service.cs
│   │   │       └── {Nome}Service.cs
│   │   ├── Acme.Sistemas.Core/  (Const, Erros, Helper, Mediators, Response, Security, Settings, DepencieInjection.cs — sem Messaging, sem Reports)
│   │   └── Acme.Sistemas.Domain/  (organização ERP mantida)
│   └── Data/
│       ├── Acme.Sistemas.Infrastructure/
│       │   ├── Databases/
│       │   │   ├── Configuration/   ← incorpora IDataConfiguration vindo de Repository
│       │   │   ├── Helper/
│       │   │   └── Migrations/
│       │   ├── Cache/  (CacheStore híbrido)
│       │   ├── Ged/
│       │   ├── Messaging/  ← absorve Core/Messaging
│       │   └── AppConfiguration/  (FeatureFlagService com hot-reload)
│       ├── Acme.Sistemas.Repository/  (sem pasta Configuration)
│       └── Acme.Sistemas.ExternalIntegration/
└── test/
    ├── Integration/Acme.Sistemas.IntegrationTest
    └── Unit/Acme.Sistemas.Services.UnitTest
```

Domain mantém categorias ERP — **decisão deliberada** documentada no blueprint local do projeto.

---

## 2. CQRS quíntuplo

### Behavior — não é boilerplate, é regra de negócio

Cada `*Behavior.cs` por funcionalidade implementa **do zero** as 4 responsabilidades:

```
┌──────────────────────────────────────────────────────────────┐
│   PIPELINE DE UM COMMAND (Mediator)                          │
│                                                              │
│  ┌────────┐  ┌──────────────┐  ┌──────────┐  ┌──────────┐    │
│  │ Request│─▶│ ValidationBeh│─▶│CacheLook │─▶│ {Func}   │    │
│  │        │  │ (FluentValid)│  │ (se Query│  │ Behavior │    │
│  │        │  │              │  │  → hit?) │  │ específ. │    │
│  └────────┘  └──────────────┘  └──────────┘  └─────┬────┘    │
│                                                    │         │
│  ┌────────┐  ┌──────────────┐  ┌──────────┐        ▼         │
│  │Response│◀─│ AuditBehavior│◀─│LogBehav. │◀─┌──────────┐    │
│  │ Result │  │ (mutações)   │  │(out)     │  │ Handler  │    │
│  └────────┘  └──────────────┘  └──────────┘  └──────────┘    │
└──────────────────────────────────────────────────────────────┘
```

**Behaviors transversais (no Core em `Mediators/Behaviors/`, registrados uma vez):**

> Convenção: `Mediators/Behaviors/` segue analogia com `Mediators/Handler/` e `Mediators/Notification/` do blueprint.

> Convenção de dependência: `Acme.Sistemas.Repository → Acme.Sistemas.Infrastructure`. Repository é a execução de comandos SQL; Infrastructure é a configuração técnica (`IDataConfiguration`, retry, transação, métricas). Sem ciclo: Infrastructure NÃO referencia Repository.

- `ValidationBehavior<TReq,TRes>` — roda `IValidator<TReq>` se houver
- `LogBehavior<TReq,TRes>` — log estruturado entrada/saída/duração
- `CacheLookupBehavior<TReq,TRes>` — para `IRequest` que implementa `ICacheable`, consulta `CacheStore` e retorna imediatamente em hit
- `AuditBehavior<TReq,TRes>` — para `IRequest` que implementa `IAuditable`, registra `AuditLog`

**Behavior específico por funcionalidade (`{Acao}CommandBehavior.cs`):**
- Validações complementares de regra de negócio (ex: "saldo do estoque suficiente", "tenant tem plano que permite NF-e")
- Enriquecimento (ex: resolver `EmpresaId` a partir de `CnpjEmissor`)
- Definir chave de cache se a operação invalida cache (ex: após `AlterarCliente`, invalidar `cliente:{tenant}:{id}`)
- Logs específicos do contexto (ex: registrar tentativa de NF-e com chave de acesso)

Behavior **nunca é vazio**. Se uma funcionalidade não tem regra extra, o Behavior ainda implementa: log estruturado do contexto + cache lookup + invalidação após escrita.

### Result — contrato de retorno padronizado

Todo Command/Query retorna `{Acao}CommandResult` ou `{Acao}QueryResult`:

```
record CriarDespesaCommandResult(
    Guid Id,
    DateTimeOffset CriadoEm,
    decimal Valor)
{
    public ResponseDefault<CriarDespesaCommandResult> Sucesso() => ...;
    public static ResponseDefault<CriarDespesaCommandResult> Erro(...) => ...;
}
```

Handlers retornam `Result` puro; o `Map.cs` do endpoint converte para `ResponseDefault<TResponse>` (usando `Response/ResponseDefault.cs` do Core).

### Cacheable / Auditable — marcadores

```
interface ICacheable { string CacheKey { get; } TimeSpan Ttl { get; } }
interface IAuditable { string Recurso { get; } string Acao { get; } }
```

Queries de leitura implementam `ICacheable`. Commands de mutação implementam `IAuditable`.

---

## 3. Endpoints 4-arquivos

### Padrão

```
Endpoints/V1/Despesa/CriarDespesa/
├── CriarDespesa.cs              implements IEndpoint, registra POST /api/v1/despesas
├── CriarDespesaRequest.cs       record do body HTTP
├── CriarDespesaResponse.cs      record da resposta HTTP
└── CriarDespesaMap.cs           Request→Command + CommandResult→Response
```

### Regras

- **Rota não muda.** Tudo que existe hoje em `DespesaEndpoints.cs` é redistribuído sem alterar URL/método/payload.
- **GETs simples sem query complexa** podem omitir `{Verbo}{Recurso}Request.cs`.
- **Listagens com paginação** têm `{Verbo}{Recurso}Request.cs` (ex: `ListarDespesasRequest.cs`) representando `?pagina=&tamanho=&busca=&ordenacao=`. Nunca `Request.cs` puro — sempre prefixado com o nome do verbo+recurso.
- O `Map.cs` é puro — nenhuma regra de negócio. Apenas espelha campos. Conversões de domínio (ex: enum→string) ficam no Map.
- O endpoint usa `IEndpoint` (já existe em `Api/Endpoints/IEndpoint.cs`) — `void MapEndpoint(IEndpointRouteBuilder app)`.

### Estratégia de migração

Por **área ERP**, em ordem decrescente de risco:

1. Configuração (poucos endpoints, baixo risco) — calibra o padrão
2. Cadastros
3. Financeiro
4. Estoque
5. Compras
6. Vendas
7. Fiscal (NF-e — mais sensível, último)
8. Auditoria/Relatórios

Cada área é um PR. Build + testes de integração precisam passar antes de seguir.

---

## 4. Cache.db (LiteDB) + MemoryCache + Redis opcional

### Arquitetura

```
┌──────────────────────────────────────────────────────────────────┐
│                    CacheStore (interface única)                  │
│   GetAsync<T>(key)  SetAsync<T>(key, value, ttl)  Remove(key)    │
└──────────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
   ┌─────────┐         ┌────────────┐         ┌──────────┐
   │ Memory  │  miss   │  LiteDB    │  miss   │  Redis   │
   │ (hot,   │ ──────▶ │  cache.db  │ ──────▶ │ (opcional│
   │  L1)    │         │  (cold,L2) │         │  L3 via  │
   │ TTL 15m │         │  TTL 15m   │         │  flag)   │
   └─────────┘         └────────────┘         └──────────┘
```

### Decisões

- **TTL default**: 15 minutos (hot e cold). Override por chamador via `ICacheable.Ttl`.
- **Posição do arquivo**: `Api/cache.db` em dev; `/tmp/cache.db` em K8s (volume `emptyDir`, per-pod), igual `Cache__LiteDbPath: "/tmp/cache.db"` do AutoProcess.
- **Cross-pod consistency**: não há. Cada pod tem seu cache. Invalidação cross-pod só ocorre quando `Cache:Provider=Redis` (flag).
- **Ativação Redis**: feature flag `Cache:Provider`. Valores `LiteDb` (default), `Redis`. Hot-reload via `IOptionsMonitor<FeatureFlagSettings>`.
- **Concorrência intra-pod**: LiteDB em modo `Connection=shared`, com `ReadLock`/`WriteLock` interno. MemoryCache não precisa de lock.
- **Eviction**: TTL absoluto. Sem LRU em LiteDB (job de limpeza por background service em `Api/Hosted/CacheCleanupWorker.cs`, intervalo 5 min).
- **Tamanho**: limite soft de 10 GB no `cache.db` (se exceder, log warning + truncate parcial das 20% entradas mais antigas). Padrão herdado do AutoProcess.

---

## 5. Feature flags em runtime

### Modelo

`featureflags.json` na raiz do `Api/`:

```json
{
  "Cache:Provider": "LiteDb",
  "Cache:HotTtlMinutes": 15,
  "Cache:ColdTtlMinutes": 15,
  "Cache:RedisConnection": "redis://...",
  "NFe:AmbienteHomologacao": true,
  "NFe:ContingenciaSvrsAuto": true,
  "Audit:Enabled": true,
  "Audit:Verbose": false
}
```

### Hot-reload

`IOptionsMonitor<FeatureFlagSettings>` com `IConfiguration.AddJsonFile("featureflags.json", reloadOnChange: true)`.

### Endpoints

- `GET /api/v1/feature-flags` — lista todas as flags ativas (permissão `feature-flags.read`)
- `GET /api/v1/feature-flags/{key}` — valor de uma flag (mesma permissão)
- `PUT /api/v1/feature-flags/{key}` — altera valor; persiste no `featureflags.json` (permissão `feature-flags.write`, restrita a admin)
- `POST /api/v1/feature-flags/recarregar` — força reload (permissão `feature-flags.write`)

Endpoints seguem o padrão 4-arquivos.

---

## 6. Realocação entre projetos

```
ANTES                                  DEPOIS
─────                                  ──────
Core/Messaging/                   →    Infrastructure/Messaging/
Core/Reports/                     →    Services/V1/Relatorios/
Infrastructure/Hosted/            →    Api/Hosted/
Infrastructure/Reports/           →    Services/V1/Relatorios/
Repository/Configuration/         →    Infrastructure/Databases/Configuration/
```

**Contrato:** depois de cada movimento, `dotnet build Atena.sln` deve passar antes de mover o próximo. Updates de namespace e using são parte do movimento, não etapa separada.

---

## 7. Convenções endurecidas (analyzers/testes)

Em `test/Unit/Acme.Sistemas.Services.UnitTest/Test/ConvencoesBlueprintTests.cs`:

- Para cada classe `*Command` em `Acme.Sistemas.Services`, deve existir `*CommandHandler`, `*CommandBehavior`, `*CommandResult`, `*CommandValidation` no mesmo namespace.
- Para cada classe `*Query`, idem.
- Para cada arquivo em `Endpoints/V*/`, se for um `*.cs` que implementa `IEndpoint`, deve existir `*Request.cs` (exceto GETs simples), `*Response.cs`, `*Map.cs` na mesma pasta.
- Falha do teste = build vermelho.

Esses testes são executados na pipeline e bloqueiam merge.

---

## 8. Sequência (alta-nível)

```
PHASE 1  ─▶  Infra (compose, kind-config) — baixo risco
PHASE 2  ─▶  Realocação entre projetos — médio
PHASE 3  ─▶  Cache híbrido + feature flags
PHASE 4  ─▶  CQRS quíntuplo (Behavior+Result em todas as funcionalidades)
PHASE 5  ─▶  Endpoints 4-arquivos (área por área)
PHASE 6  ─▶  Convenções endurecidas + docs
```

Cada phase é um conjunto de tasks no `tasks.md`. Build verde + testes verdes obrigatórios entre phases.

---

## 9. Riscos e mitigações

| Risco | Mitigação |
|---|---|
| Volume mecânico de Phase 5 (~600 arquivos) gera bugs sutis | Migrar área por área, com testes de integração rodando 100% após cada área. Smoke test manual da rota afetada. |
| Behaviors mal escritos cacheam dados inválidos | Cache lookup só em queries marcadas `ICacheable`; invalidação obrigatória em commands que mutam o mesmo recurso (registrado no Behavior). Teste de "stale cache after write" obrigatório por funcionalidade. |
| LiteDB corrompido em produção (concorrência) | Arquivo per-pod (`/tmp`); recriado a cada start; nunca compartilhado via PVC. Já validado pelo AutoProcess. |
| Movimentar Hosted/Reports quebra DI | Cada movimento é um commit isolado com build verde antes do próximo. |
| Endpoints com 4-arquivos viram código repetitivo (Map.cs trivial) | Aceitar; é o preço do padrão. Considerar gerador `dotnet new` em fase posterior (fora desta change). |
| Domain ERP "fora do padrão" gera dúvida em revisão | Justificativa documentada no `CLAUDE.md` e no proposal. Padrão Acme exige Domain por projeto, não Domain compartilhado. |

---

## 10. Não-objetivos (fora do escopo)

- Reorganizar o **Domain** do Atena (categorias ERP permanecem).
- Criar template `dotnet new` automatizado.
- Implementar Outbox Pattern (já existe parcialmente em `ProtocolIntegrationOutboxEntity`; AutoProcess território).
- Adicionar BPMN/Workflow Engine.
- Migrar de SQL puro para EF Core ou vice-versa.
- Reorganizar testes existentes.
