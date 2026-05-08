## Why

A change `aderencia-blueprint-acme` deixou 16 arquivos `*Endpoints.cs` (monoliths) registrando múltiplas rotas cada — total de **82 rotas dentro de monoliths**. O analyzer `ConvencoesBlueprintTests` (Fase 8) passa porque itera `IEndpoint` (1 monólito = 1 classe), não rotas individuais. **Mas o blueprint Acme exige uma pasta por verbo HTTP**, com `{Verbo}{Recurso}.cs` + `Request` (opcional) + `Response` + `Map`.

Essa dívida foi registrada explicitamente no `tasks.md` da Fase 7 como decisão deliberada — split de ~245 arquivos thin com risco de regressão de routing alto sem cuidado dedicado. Agora é o momento de pagar.

## What Changes

- **Quebra mecânica de cada monólito** em pastas por verbo HTTP, preservando 100% das rotas (path/verb/permissions/name/handler).
- **Endurecimento do analyzer** Phase 8: `TodoEndpoint_TemResponseEMap` passa a iterar **rotas registradas** (via `IRouteEndpointDataSource` ou similar) em vez de classes `IEndpoint`, garantindo que cada rota tenha sua pasta.
- **Snapshot diff de rotas** antes/depois deve ser vazio.
- Sem mudança de contrato HTTP, sem mudança de domain, sem mudança de pipeline.

## Capabilities

### Modified Capabilities

Nenhuma — esta change não muda comportamento, apenas organização física dos arquivos. Não há specs delta.

## Out of Scope

- Adoção de `ICacheable` em queries (responsabilidade de outra change futura).
- Cache invalidation real nos `*Behavior.cs` (idem; só faz sentido após queries cacheáveis).
- Refactor do pipeline transversal — está estável (Phase 3).
- Split do frontend Angular — outro escopo.

## Risks

- **Regressão de routing**: typo em path/permissão/name no split. Mitigação: snapshot diff via test que enumera `app.Endpoints` em runtime.
- **Tagging Swagger inconsistente**: monoliths usavam `MapGroup("/api/v1/x").WithTags("X")`. No split, cada endpoint precisa de `.WithTags("X")` próprio. Mitigação: revisar via Swagger gen output.
- **Permissions duplicadas**: alguns monolíticos aplicam `RequireAuthorization()` no group; cada split precisa repetir explicitamente. Mitigação: revisar `RequirePermissao` por endpoint.

## Success Criteria

- 0 monoliths `*Endpoints.cs` no `Endpoints/V1/`
- 100% das ~120 rotas em pastas individuais
- `dotnet test` (unit + integration HealthCheck) verde
- Snapshot de rotas idêntico ao baseline (mesma string set de paths/verbs/names)
- `ConvencoesBlueprintTests` endurecido passa para todas as rotas
