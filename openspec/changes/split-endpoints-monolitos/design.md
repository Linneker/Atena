# Design — split-endpoints-monolitos

## Estratégia

Refactor **manual por monólito**, guardado por snapshot oracle de rotas. Decisão (2026-05-07) de descartar gerador automático: o ROI de escrever + debugar parser Python é negativo dado o volume (16 monoliths) e a irregularidade dos chains. Cada monólito leva 30-90 min manual, é commit-revisável e mantém 100% de fidelidade.

### Fase 1 — Snapshot de baseline (test)

Adicionar um teste de integração que **enumera as rotas reais** registradas pelo `WebApplication`:

```csharp
var dataSource = app.Services.GetRequiredService<EndpointDataSource>();
var routes = dataSource.Endpoints
    .OfType<RouteEndpoint>()
    .Select(e => new {
        Pattern = e.RoutePattern.RawText,
        Verbs = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods,
        Name = e.DisplayName
    });
```

Salvar como JSON ordenado em `openspec/changes/split-endpoints-monolitos/baseline/routes-runtime.json`. Esse é o oráculo definitivo; o snapshot por `grep` da Phase 7 é apenas estimativa.

### Fase 2 — Split manual por monólito

Para cada `*Endpoints.cs`, o procedimento humano é:

1. **Mapear** cada `<groupVar>.Map<Verb>("/path", lambda).WithName("Nome").Produces<...>()...;` no monólito.
2. **Resolver** o `groupVar` de volta para `app.MapGroup("/prefix").RequireAuthorization().WithTags("X")` no topo do método.
3. **Para cada rota**: criar pasta `Endpoints/V1/<Recurso>/<Verbo><Recurso>/` com:
   - `<Verbo><Recurso>Endpoint.cs` — classe `IEndpoint` recriando a rota com URL completa (prefix + path) e chains preservados.
   - `<Verbo><Recurso>Response.cs` — record/alias do tipo Result.
   - `<Verbo><Recurso>Map.cs` — extensions Request↔Command (ou marker se mapping inline).
4. **Remover** o monólito após todas as rotas extraídas.
5. **Validar**: `dotnet build` + test `RotasEnumeradas_BatemComBaseline` verdes.
6. **Commit** dedicado por monólito (16 commits no total) — permite bisect.

### Fase 3 — Limpeza dos markers órfãos

Os 16 `*EndpointsResponse.cs` + 16 `*EndpointsMap.cs` criados pela Phase 7 do change anterior viram inúteis após o split — removê-los explicitamente.

### Fase 4 — Endurecer analyzer

`TodoEndpoint_TemResponseEMap` em `ConvencoesBlueprintTests`: ao invés de iterar `IEndpoint` types, iterar rotas reais via `EndpointDataSource`. Para cada rota, exigir que o arquivo `Endpoint.cs` que a registra esteja em pasta com siblings `Response.cs` + `Map.cs`.

## Decisões e tradeoffs

### Por que não MapGroup nas pastas filhas?

Cada split recriaria `app.MapGroup(prefix).WithTags(...)` localmente, mas isso duplicaria a chamada `MapGroup` 5-10× para o mesmo prefix. **Não é problema**: cada `IEndpoint.MapEndpoint(app)` é independente; o ASP.NET aceita múltiplos `MapGroup("/api/v1/x")` apontando para o mesmo prefix. A leve repetição é o preço da modularidade.

Alternativa rejeitada: manter um `XGroupRegistration : IEndpoint` que cria o group e endpoints filhos pegam o group de DI. Isso reintroduz acoplamento entre arquivos — exatamente o que o split combate.

### Por que abandonar o gerador?

Avaliado em 2026-05-07: 16 monoliths, ~83 rotas, padrões irregulares (alguns têm `MapGroup` aninhado, chains com `RequireAuthorization` no group + `RequirePermissao` na rota, mistura de inline lambda com handler delegate). Para cobrir 95% dos casos o gerador precisaria ~6h; para os 5% restantes (revisão + correção manual dos casos esquisitos) mais 4h. Total: ~10h. Split manual: ~30-90 min × 16 = 8-24h, mas com confiança total e revisão visual de cada commit. Em uma equipe pequena, **manual ganha pelo zero-risk de regressão silenciosa**.

### Test runtime vs analyzer estático

O snapshot de rotas runtime é mais confiável (vê o que realmente está registrado), mas requer subir `WebApplicationFactory`. O analyzer estático é rápido mas pode mentir (uma rota registrada via método externo passa despercebida). **Adotamos ambos**: analyzer no test rápido (CI obrigatório), runtime no test de integração (CI opcional, requer Docker).

## Inventário a quebrar

| Monólito                          | Rotas | Recurso(s) afetado(s) |
|-----------------------------------|-------|------------------------|
| ComprasEndpoints                  | 10    | Solicitacao, PedidoCompra, Recebimento |
| VendasEndpoints                   | 7     | Orcamento, PedidoVenda, Faturamento, DevolucaoVenda |
| ProdutosEndpoints                 | 6     | Produto |
| EstoqueEndpoints                  | 6     | Estoque, Inventario |
| ClientesEndpoints                 | 6     | Cliente |
| RolesEndpoints                    | 5     | Role, Permission |
| FornecedoresEndpoints             | 5     | Fornecedor |
| DividasEndpoints                  | 5     | Divida |
| DashboardEndpoints                | 5     | Dashboard, Aging, PosicaoEstoque |
| TiposProdutoEndpoints             | 4     | TipoProduto, TipoValorProduto |
| RelatoriosFinanceirosEndpoints    | 4     | Balanco, DRE |
| PlanoDeContasEndpoints            | 4     | PlanoDeContas |
| FuncionariosEndpoints             | 4     | Funcionario |
| ContasReceberEndpoints            | 4     | ContaReceber |
| ContasPagarEndpoints              | 4     | ContaPagar |
| CentrosDeCustoEndpoints           | 4     | CentroDeCusto |
| **Total**                         | **82**| ~24 recursos          |

Output esperado: 82 pastas × 3 arquivos = **246 arquivos novos**, 16 monoliths removidos, 32 markers `*EndpointsResponse.cs`/`*EndpointsMap.cs` removidos (que viraram irrelevantes). Saldo líquido: **+198 arquivos**.
