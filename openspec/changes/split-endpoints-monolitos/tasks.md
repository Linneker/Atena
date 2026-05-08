# Tasks — split-endpoints-monolitos

> Cada monólito = 1 commit lógico revisável. Critério por commit: build verde + `RotasEnumeradas_BatemComBaseline` verde.
>
> **Decisão (2026-05-07)**: split é **manual**, não via gerador. Monoliths atuais têm padrões irregulares que tornariam o gerador frágil; 16 monoliths × 30-90 min cada é mais barato que escrever + debugar gerador Python.

---

## Fase 1 — Snapshot oracle de rotas

- [ ] 1.1 Criar `test/Integration/Acme.Sistemas.IntegrationTest/Test/RouteSnapshotTests.cs`
- [ ] 1.2 Implementar método que sobe `WebApplicationFactory`, obtém `EndpointDataSource`, materializa lista ordenada de `{ Pattern, Verbs[], DisplayName }`
- [ ] 1.3 Serializar como JSON ordenado (chaves sorted) com `JsonSerializerOptions { WriteIndented = true }`
- [ ] 1.4 Salvar baseline em `openspec/changes/split-endpoints-monolitos/baseline/routes-runtime.json`
- [ ] 1.5 Test `RotasEnumeradas_BatemComBaseline` carrega baseline + compara com snapshot atual; falha em qualquer diff
- [ ] 1.6 Rodar localmente, gerar baseline inicial, commitar `baseline/routes-runtime.json`
- [ ] 1.7 CI: garantir que test entra no ciclo de integração (requer Docker)

---

## Fase 2 — Split manual dos monoliths (ordem crescente de complexidade)

> Para CADA monólito abaixo, o procedimento padrão é:
> 1. Ler o arquivo monólito e mapear cada `Map<Verb>(...)` → nome de rota
> 2. Para cada rota: criar pasta `Endpoints/V1/<Recurso>/<Verbo><Recurso>/` com `<Verbo><Recurso>Endpoint.cs`, `<Verbo><Recurso>Response.cs`, `<Verbo><Recurso>Map.cs`
> 3. `Endpoint.cs` recria a rota com URL completa (prefix + path) + chains preservados (RequireAuthorization, RequirePermissao, WithTags, Produces)
> 4. Remover o monólito
> 5. `dotnet build` + `dotnet test --filter RouteSnapshotTests` verde
> 6. Commit `refactor(endpoints): split <Monolith> em pastas por verbo`

### 2.1 — CentrosDeCustoEndpoints (4 rotas) — calibrador
- [ ] 2.1.1 Mapear as 4 rotas do monólito (verbo, path, name, permissão)
- [ ] 2.1.2 Criar 4 pastas + 12 arquivos
- [ ] 2.1.3 Remover monólito
- [ ] 2.1.4 Build + RouteSnapshot verde
- [ ] 2.1.5 Commit dedicado
- [ ] 2.1.6 **Code review humano** dos 12 arquivos — calibrar padrão antes de seguir

### 2.2 — ContasPagarEndpoints (4 rotas)
- [ ] 2.2.1 Mapear rotas
- [ ] 2.2.2 Gerar pastas + arquivos
- [ ] 2.2.3 Remover monólito + build/test verde + commit

### 2.3 — ContasReceberEndpoints (4 rotas)
- [ ] 2.3.1 Mapear rotas
- [ ] 2.3.2 Gerar pastas + arquivos
- [ ] 2.3.3 Remover monólito + build/test verde + commit

### 2.4 — FuncionariosEndpoints (4 rotas)
- [ ] 2.4.1 Mapear rotas
- [ ] 2.4.2 Gerar pastas + arquivos
- [ ] 2.4.3 Remover monólito + build/test verde + commit

### 2.5 — PlanoDeContasEndpoints (4 rotas)
- [ ] 2.5.1 Mapear rotas
- [ ] 2.5.2 Gerar pastas + arquivos
- [ ] 2.5.3 Remover monólito + build/test verde + commit

### 2.6 — RelatoriosFinanceirosEndpoints (4 rotas, 2 áreas: Balanço + DRE)
- [ ] 2.6.1 Mapear rotas e separar por área lógica
- [ ] 2.6.2 Gerar pastas (provavelmente sob `Relatorios/Balanco/...` e `Relatorios/Dre/...`)
- [ ] 2.6.3 Remover monólito + build/test verde + commit

### 2.7 — TiposProdutoEndpoints (4 rotas, 2 áreas: TipoProduto + TipoValorProduto)
- [ ] 2.7.1 Mapear rotas e separar por entidade
- [ ] 2.7.2 Gerar pastas
- [ ] 2.7.3 Remover monólito + build/test verde + commit

### 2.8 — DashboardEndpoints (5 rotas, 3 áreas: Dashboard + Aging + PosicaoEstoque)
- [ ] 2.8.1 Mapear rotas e separar por capability
- [ ] 2.8.2 Gerar pastas (`Dashboard/...`, `Aging/...`, `PosicaoEstoque/...`)
- [ ] 2.8.3 Remover monólito + build/test verde + commit

### 2.9 — DividasEndpoints (5 rotas)
- [ ] 2.9.1 Mapear rotas
- [ ] 2.9.2 Gerar pastas + arquivos
- [ ] 2.9.3 Remover monólito + build/test verde + commit

### 2.10 — FornecedoresEndpoints (5 rotas)
- [ ] 2.10.1 Mapear rotas
- [ ] 2.10.2 Gerar pastas + arquivos
- [ ] 2.10.3 Remover monólito + build/test verde + commit

### 2.11 — RolesEndpoints (5 rotas, 2 áreas: Role + Permission)
- [ ] 2.11.1 Mapear rotas e separar por entidade
- [ ] 2.11.2 Gerar pastas
- [ ] 2.11.3 Remover monólito + build/test verde + commit

### 2.12 — ClientesEndpoints (6 rotas)
- [ ] 2.12.1 Mapear rotas
- [ ] 2.12.2 Gerar pastas + arquivos
- [ ] 2.12.3 Remover monólito + build/test verde + commit

### 2.13 — EstoqueEndpoints (6 rotas, 2 áreas: Estoque + Inventario)
- [ ] 2.13.1 Mapear rotas; atenção a regras de saldo real-time (não mexer em handler)
- [ ] 2.13.2 Gerar pastas (`Estoque/...`, `Inventario/...`)
- [ ] 2.13.3 Remover monólito + build/test verde + commit

### 2.14 — ProdutosEndpoints (6 rotas)
- [ ] 2.14.1 Mapear rotas
- [ ] 2.14.2 Gerar pastas + arquivos
- [ ] 2.14.3 Remover monólito + build/test verde + commit

### 2.15 — VendasEndpoints (7 rotas, 4 áreas: Orcamento + PedidoVenda + Faturamento + DevolucaoVenda)
- [ ] 2.15.1 Mapear rotas e separar por entidade
- [ ] 2.15.2 Gerar pastas (`Vendas/Orcamento/...`, `Vendas/PedidoVenda/...`, `Vendas/Faturamento/...`, `Vendas/DevolucaoVenda/...`)
- [ ] 2.15.3 Remover monólito + build/test verde + commit

### 2.16 — ComprasEndpoints (10 rotas, 3 áreas: Solicitacao + PedidoCompra + Recebimento) — último, maior
- [ ] 2.16.1 Mapear rotas e separar por entidade
- [ ] 2.16.2 Gerar pastas (`Compras/SolicitacaoCompra/...`, `Compras/PedidoCompra/...`, `Compras/RecebimentoCompra/...`)
- [ ] 2.16.3 Remover monólito + build/test verde + commit

---

## Fase 3 — Limpeza de markers órfãos

- [ ] 3.1 Listar todos `*EndpointsResponse.cs` em `Endpoints/V1/` (markers gerados pela Phase 7 do `aderencia-blueprint-acme`)
- [ ] 3.2 Listar todos `*EndpointsMap.cs` análogos
- [ ] 3.3 Deletar os 16 `*EndpointsResponse.cs` órfãos
- [ ] 3.4 Deletar os 16 `*EndpointsMap.cs` órfãos
- [ ] 3.5 `dotnet build` verde após limpeza
- [ ] 3.6 Commit `chore(endpoints): remove markers órfãos pós-split`

---

## Fase 4 — Endurecimento do analyzer

- [ ] 4.1 Localizar `ConvencoesBlueprintTests.TodoEndpoint_TemResponseEMap` no projeto Unit Tests
- [ ] 4.2 Substituir iteração por `IEndpoint` types por iteração via `EndpointDataSource.Endpoints`
- [ ] 4.3 Para cada `RouteEndpoint`, resolver arquivo de origem do handler (via `DisplayName` ou metadata) e validar siblings `*Response.cs` + `*Map.cs` na mesma pasta
- [ ] 4.4 Adicionar exceções explícitas (Swagger, Health, etc) em allow-list se necessário
- [ ] 4.5 Test passa para todas as ~120 rotas
- [ ] 4.6 Commit `test(blueprint): endurece analyzer pra iterar EndpointDataSource`

---

## Fase 5 — Documentação

- [ ] 5.1 Atualizar `CLAUDE.md` confirmando que 100% dos endpoints seguem padrão 4-arquivos
- [ ] 5.2 Remover qualquer nota de "dívida técnica de monoliths" no `CLAUDE.md`
- [ ] 5.3 Atualizar `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md` se houver menção a monoliths
- [ ] 5.4 Commit `docs: confirma 100% aderência blueprint pós-split`

---

## Fase 6 — Validação final

- [ ] 6.1 `dotnet build Atena.sln` verde sem warnings novos
- [ ] 6.2 `dotnet test` (unit) verde — incluindo analyzer endurecido
- [ ] 6.3 `dotnet test` (integration HealthCheck + RouteSnapshot) verde
- [ ] 6.4 Validar: 0 arquivos `*Endpoints.cs` em `Endpoints/V1/`
- [ ] 6.5 Validar: ~120 arquivos `*Endpoint.cs` em pastas filhas (mindepth 2)
- [ ] 6.6 `openspec validate split-endpoints-monolitos --strict` verde
