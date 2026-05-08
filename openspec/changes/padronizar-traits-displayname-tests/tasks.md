# Tasks — padronizar-traits-displayname-tests

> Critério de "verde" por commit: `dotnet build` + `dotnet test` da camada tocada verdes. O analyzer só liga depois do retrofit completo (Fase 4).

---

## Fase 1 — Documentação do padrão

- [x] 1.1 Adicionar seção "Convenções de testes" no `documentacao/blueprint.yml` documentando Trait("Solucao") + Trait("Acao") + DisplayName
- [x] 1.2 Adicionar seção análoga em `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md` (com exemplo Given-When-Then)
- [x] 1.3 Atualizar `CLAUDE.md` com bloco curto sobre o padrão de testes (~10 linhas)
- [x] 1.4 Commit `docs(blueprint): padrão de Trait + DisplayName em testes`

---

## Fase 2 — Analyzer (escrito mas inativo até Fase 4)

- [x] 2.1 Adicionar método `TodoTeste_TemDisplayNameESolucaoEAcao` em `ConvencoesBlueprintTests`
- [x] 2.2 Implementar reflexão sobre assembly Unit + Integration para coletar `[Fact]`/`[Theory]` (incluindo `Skip`); ProjectReference Unit→Integration adicionada para acesso direto ao assembly
- [x] 2.3 Validar DisplayName não-vazio, Trait("Solucao") na allow-list, Trait("Acao") não-vazio (lendo via `CustomAttributeData` — `TraitAttribute` não expõe Name/Value como properties)
- [x] 2.4 Marcar como `[Fact(Skip = "ativa após retrofit completo — Fase 4")]` com Trait+DisplayName próprios
- [x] 2.5 Build verde + commit `test(blueprint): adiciona analyzer de Trait+DisplayName (skip até Fase 4)`

---

## Fase 3 — Retrofit dos 22 arquivos / ~58 fatos

> Por commit: 1 camada (Solucao). Mantém histórico legível e bisect rápido.

### 3.1 — Services / Behaviors (5 arquivos)
- [ ] 3.1.1 `AuditBehaviorTests.cs` — `Acao=AuditBehavior`
- [ ] 3.1.2 `CacheLookupBehaviorTests.cs` — `Acao=CacheLookupBehavior`
- [ ] 3.1.3 `LogBehaviorTests.cs` — `Acao=LogBehavior`
- [ ] 3.1.4 `ValidationBehaviorTests.cs` — `Acao=ValidationBehavior`
- [ ] 3.1.5 `PipelineBehaviorOrderingTests.cs` — `Acao=PipelineBehavior`
- [ ] 3.1.6 Build + test verde + commit `test(services): retrofit Trait+DisplayName em behaviors`

### 3.2 — Services / Handlers (3 arquivos)
- [ ] 3.2.1 `LoginCommandHandlerTests.cs` — `Acao=Login`
- [ ] 3.2.2 `CriarClienteCommandHandlerTests.cs` — `Acao=CriarCliente`
- [ ] 3.2.3 `BaixarDespesaCommandHandlerTests.cs` — `Acao=BaixarDespesa`
- [ ] 3.2.4 Build + test verde + commit `test(services): retrofit Trait+DisplayName em handlers`

### 3.3 — Core / Helpers (2 arquivos)
- [ ] 3.3.1 `JwtTokenServiceTests.cs` — `Solucao=Core, Acao=JwtTokenService`
- [ ] 3.3.2 `PasswordHelperTests.cs` — `Solucao=Core, Acao=PasswordHelper`
- [ ] 3.3.3 Build + test verde + commit `test(core): retrofit Trait+DisplayName em helpers`

### 3.4 — Infrastructure (4 arquivos)
- [ ] 3.4.1 `CacheCleanupWorkerTests.cs` — `Acao=CacheCleanupWorker`
- [ ] 3.4.2 `CacheProviderRouterTests.cs` — `Acao=CacheProviderRouter`
- [ ] 3.4.3 `FeatureFlagServiceTests.cs` — `Acao=FeatureFlagService`
- [ ] 3.4.4 `HybridCacheStoreTests.cs` — `Acao=HybridCacheStore`
- [ ] 3.4.5 Build + test verde + commit `test(infra): retrofit Trait+DisplayName`

### 3.5 — Repository (1 arquivo)
- [ ] 3.5.1 `BaseRepositoryTenantFilterTests.cs` — `Acao=TenantFilter`
- [ ] 3.5.2 Build + test verde + commit `test(repository): retrofit Trait+DisplayName`

### 3.6 — Api / Integration + Http (6 arquivos)
- [ ] 3.6.1 `HttpTenantContextAccessorTests.cs` — `Acao=TenantContext`
- [ ] 3.6.2 `HealthCheckTests.cs` — `Acao=HealthCheck`
- [ ] 3.6.3 `FluxoVendaCompletaTests.cs` — `Acao=FluxoVenda`
- [ ] 3.6.4 `IsolamentoCrossTenantTests.cs` — `Acao=IsolamentoCrossTenant`
- [ ] 3.6.5 `RouteSnapshotTests.cs` — `Acao=RouteSnapshot`
- [ ] 3.6.6 `EndpointConventionTests.cs` — `Acao=Convencoes`
- [ ] 3.6.7 Build + test verde + commit `test(api): retrofit Trait+DisplayName`

### 3.7 — Test / meta (1 arquivo)
- [ ] 3.7.1 `ConvencoesBlueprintTests.cs` — `Solucao=Test, Acao=Convencoes` em todos os 4+ fatos (inclui o analyzer ainda Skipped)
- [ ] 3.7.2 Build + test verde + commit `test(meta): retrofit Trait+DisplayName em ConvencoesBlueprintTests`

---

## Fase 4 — Validação final

- [ ] 4.1 Remover `Skip` do analyzer `TodoTeste_TemDisplayNameESolucaoEAcao`
- [ ] 4.2 `dotnet test` (unit) verde — analyzer agora ativo
- [ ] 4.3 Validar filtro: `dotnet test --filter "Trait=Solucao=Services"` retorna apenas testes da camada
- [ ] 4.4 Validar filtro: `dotnet test --filter "Trait=Acao=CriarCliente"` retorna apenas testes da unidade
- [ ] 4.5 `openspec validate padronizar-traits-displayname-tests --strict` verde
- [ ] 4.6 Commit `test(blueprint): ativa analyzer de Trait+DisplayName após retrofit`
