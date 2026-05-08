# Design — padronizar-traits-displayname-tests

## Padrão de attributes

```csharp
[Trait("Solucao", "Services")]
[Trait("Acao", "CriarDespesa")]
[Fact(DisplayName = "Dado dados válidos, quando criar despesa, então persiste e retorna 201")]
public async Task CriarDespesa_DadosValidos_Retorna201()
{
    // ...
}
```

### Solucao — vocabulário fechado

| Valor                 | Quando usar                                                              |
|-----------------------|--------------------------------------------------------------------------|
| `Api`                 | Testes de endpoints, integração HTTP, middlewares, host                  |
| `Services`            | Handlers de Command/Query/Notification, behaviors do pipeline            |
| `Core`                | Helpers/utilitários puros do `Acme.Sistemas.Core` (Jwt, Hash, Password)  |
| `Domain`              | Entidades, value objects, regras de invariante                           |
| `Repository`          | Repositórios SQL, filtro de tenant                                       |
| `Infrastructure`      | Cache, mensageria, email, GED, hosted services                           |
| `ExternalIntegration` | HttpClientProxy, ViaCEP, integrações externas                            |
| `Test`                | Meta-tests (convenções, blueprint, layout)                               |

Mudou camada → muda allow-list no analyzer. Decisão deliberada: a lista vive no test, não em config externa.

### Acao — vocabulário aberto

Nome curto da unidade-em-teste, em PascalCase, sem sufixos:

| Tipo de unidade            | Exemplo de Acao                                          |
|----------------------------|----------------------------------------------------------|
| Command                    | `CriarDespesa`, `Login`, `BaixarDespesa`                 |
| Query                      | `ListarLogs`, `ObterFluxo`, `GerarBalanco`               |
| Behavior do pipeline       | `AuditBehavior`, `CacheLookupBehavior`, `LogBehavior`    |
| Service/Helper             | `JwtTokenService`, `PasswordHelper`, `FeatureFlagService`|
| Worker / Hosted            | `CacheCleanupWorker`                                     |
| Repository / Filtro        | `TenantFilter`                                           |
| Aspecto de Api             | `HealthCheck`, `RouteSnapshot`, `IsolamentoCrossTenant`  |
| Meta / convenções          | `Convencoes`                                             |

Exceções documentadas:
- `PipelineBehaviorOrderingTests` → `Acao = "PipelineBehavior"` (testa a ordem dos 4 behaviors transversais).
- `ConvencoesBlueprintTests` (e o novo Trait analyzer) → `Acao = "Convencoes"`, `Solucao = "Test"`.

### DisplayName — Given-When-Then em PT

Forma canônica: `"Dado <contexto>, quando <ato>, então <resultado>"`.

Variações aceitas:
- Pode omitir "Dado <contexto>" se o contexto é trivial: `"Quando login com credenciais inválidas, então retorna 401"`.
- Pode usar "Deve <comportamento>" como prefixo se o cenário é estado-livre: `"Deve gerar hash diferente para mesma senha em chamadas distintas"`.
- Sempre PT-BR.

Não é frase: `"Login retorna 401"` ❌. Frase: `"Quando login com senha errada, então retorna 401"` ✅.

## Analyzer

### Onde mora

Novo `[Fact]` em `test/Unit/Acme.Sistemas.Services.UnitTest/Test/ConvencoesBlueprintTests.cs`. Não é integration (não precisa Docker). Reflexão sobre os assemblies de **Unit** e **Integration**.

### O que valida

```csharp
[Trait("Solucao", "Test")]
[Trait("Acao", "Convencoes")]
[Fact(DisplayName = "Todo método [Fact]/[Theory] tem DisplayName + Trait(Solucao) + Trait(Acao) válidos")]
public void TodoTeste_TemDisplayNameESolucaoEAcao()
{
    var assemblies = new[] { typeof(ConvencoesBlueprintTests).Assembly, typeof(EndpointConventionTests).Assembly };
    var faltando = new List<string>();
    foreach (var asm in assemblies)
    {
        foreach (var type in asm.GetTypes().Where(IsTestClass))
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                var fact = method.GetCustomAttribute<FactAttribute>(); // pega Fact e Theory (Theory : Fact)
                if (fact is null) continue;

                if (string.IsNullOrWhiteSpace(fact.DisplayName))
                    faltando.Add($"{type.FullName}.{method.Name}: faltando DisplayName");

                var traits = method.GetCustomAttributes<TraitAttribute>().ToList();
                if (!traits.Any(t => t.Name == "Solucao" && CamadasValidas.Contains(t.Value)))
                    faltando.Add($"{type.FullName}.{method.Name}: faltando [Trait(\"Solucao\", <camada>)]");
                if (!traits.Any(t => t.Name == "Acao" && !string.IsNullOrWhiteSpace(t.Value)))
                    faltando.Add($"{type.FullName}.{method.Name}: faltando [Trait(\"Acao\", <unidade>)]");
            }
        }
    }
    faltando.Should().BeEmpty(string.Join("\n", faltando));
}
```

### Pontos sutis

- `[Theory]` herda de `[Fact]`, então `GetCustomAttribute<FactAttribute>()` pega ambos. `[Theory(DisplayName = "...")]` também funciona.
- `[Fact(Skip = "...")]` continua sujeito ao analyzer — Skip não isenta padrão.
- xUnit oferece `TraitAttribute` própria — não usar custom. `[Trait("name", "value")]` é o que xUnit já reconhece pra `--filter Trait=...`.

### Falsos positivos potenciais

- Test classes geradas por código (não há).
- Métodos `private` com `[Fact]` (xUnit ignora — analyzer também filtra por `IsPublic`).

## Retrofit — ordem por camada

Ordem deliberada do mais cohesivo (1 arquivo, 1 ação) ao mais transversal (1 arquivo, vários traits):

```
Fase 3.1 — Services / Behaviors      (5 arquivos, ~19 fatos)
Fase 3.2 — Services / Handlers       (3 arquivos, ~11 fatos)
Fase 3.3 — Core / Helpers            (2 arquivos, ~9 fatos)
Fase 3.4 — Infrastructure            (4 arquivos, ~11 fatos)
Fase 3.5 — Repository                (1 arquivo,  ~3 fatos)
Fase 3.6 — Api (Integration + Http)  (6 arquivos, ~10 fatos)
Fase 3.7 — Test (meta)               (1 arquivo,   4 fatos)
```

`ConvencoesBlueprintTests` em si (Fase 3.7) é o último — o analyzer da Fase 2 já foi escrito mas roda só na Fase 4 (validação final). Isso evita o paradoxo de um test que reprova a si mesmo antes de estar conforme.

## Decisões e tradeoffs

### Por que Solucao = camada e não módulo de negócio?

Discutido na exploração. Camada acopla testes ao layout físico do `src/`. Módulo de negócio (Financeiro, Estoque) acopla a um modelo de domínio que pode mudar mais. **Camada estabiliza o vocabulário** — ela só muda em refactor arquitetural (mudança de Clean Architecture pra hexagonal, p.ex.), evento que justifica revisitar a convenção. Módulo muda toda vez que reorganizamos pastas dentro de Services.

### Por que `[Fact(DisplayName = ...)]` e não `[Fact]` + atributo separado?

Forma nativa de xUnit. Não inventar. Test Explorer e `dotnet test --logger "console;verbosity=detailed"` renderizam DisplayName direto.

### Por que não Trait("Categoria", "Unit"|"Integration")?

Redundante com a separação por projeto (`Acme.Sistemas.Services.UnitTest` × `Acme.Sistemas.IntegrationTest`). Filtro `dotnet test test/Unit/...` já entrega o mesmo resultado. Adicionar Trait dobraria informação sem ganho.

### Por que a allow-list de camadas mora dentro do test?

Mover para JSON externo / config / blueprint.yml seria over-engineering. Mudar camada arquitetural é evento raro o suficiente pra valer code-review explícito no test.
