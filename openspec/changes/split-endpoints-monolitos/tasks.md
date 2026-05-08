# Tasks — split-endpoints-monolitos

> Cada monólito = 1 commit lógico revisável. Critério por commit: build verde + `RotasEnumeradas_BatemComBaseline` verde.
>
> **Decisão (2026-05-07)**: split é **manual**, não via gerador. Monoliths atuais têm padrões irregulares que tornariam o gerador frágil; 16 monoliths × 30-90 min cada é mais barato que escrever + debugar gerador Python.

---

## Fase 1 — Snapshot oracle de rotas

- [x] 1.1 Criar `test/Integration/Acme.Sistemas.IntegrationTest/Test/RouteSnapshotTests.cs`
- [x] 1.2 Implementar método que sobe `WebApplicationFactory`, obtém `EndpointDataSource`, materializa lista ordenada de `{ Pattern, Verbs[], DisplayName }`
- [x] 1.3 Serializar como JSON ordenado (chaves sorted) com `JsonSerializerOptions { WriteIndented = true }`
- [x] 1.4 Salvar baseline em `openspec/changes/split-endpoints-monolitos/baseline/routes-runtime.json`
- [x] 1.5 Test `RotasEnumeradas_BatemComBaseline` carrega baseline + compara com snapshot atual; falha em qualquer diff
- [x] 1.6 Rodar localmente, gerar baseline inicial, commitar `baseline/routes-runtime.json`
- [x] 1.7 CI: garantir que test entra no ciclo de integração (requer Docker)

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
- [x] 2.1.1 Mapear as 4 rotas do monólito (verbo, path, name, permissão)
- [x] 2.1.2 Criar 4 pastas + 12 arquivos
- [x] 2.1.3 Remover monólito
- [x] 2.1.4 Build + RouteSnapshot verde
- [x] 2.1.5 Commit dedicado (superseded: split entrou no histórico via commit bulk `71b7969 corrigindo fase 2 e 3`; intenção de preservar a separação no histórico ficou prejudicada, mas conteúdo está aplicado e validado pelo RouteSnapshot)
- [x] 2.1.6 **Code review humano** dos 12 arquivos — calibrar padrão antes de seguir (concluído: padrão Request/Response/Map confirmado + trailing slash normalizado)

### 2.2 — ContasPagarEndpoints (4 rotas)
- [x] 2.2.1 Mapear rotas
- [x] 2.2.2 Gerar pastas + arquivos
- [x] 2.2.3 Remover monólito + build/test verde + commit

### 2.3 — ContasReceberEndpoints (4 rotas)
- [x] 2.3.1 Mapear rotas
- [x] 2.3.2 Gerar pastas + arquivos
- [x] 2.3.3 Remover monólito + build/test verde + commit

### 2.4 — FuncionariosEndpoints (4 rotas)
- [x] 2.4.1 Mapear rotas
- [x] 2.4.2 Gerar pastas + arquivos
- [x] 2.4.3 Remover monólito + build/test verde + commit

### 2.5 — PlanoDeContasEndpoints (4 rotas)
- [x] 2.5.1 Mapear rotas
- [x] 2.5.2 Gerar pastas + arquivos
- [x] 2.5.3 Remover monólito + build/test verde + commit

### 2.6 — RelatoriosFinanceirosEndpoints (4 rotas, 2 áreas: Balanço + DRE)
- [x] 2.6.1 Mapear rotas e separar por área lógica
- [x] 2.6.2 Gerar pastas (`Relatorios/Dre/GerarDre`, `Relatorios/Dre/GerarDrePdf`, `Relatorios/Balanco/GerarBalanco`, `Relatorios/Balanco/GerarBalancoPdf`)
- [x] 2.6.3 Remover monólito + build/test verde + commit

### 2.7 — TiposProdutoEndpoints (4 rotas, 2 áreas: TipoProduto + TipoValorProduto)
- [x] 2.7.1 Mapear rotas e separar por entidade
- [x] 2.7.2 Gerar pastas (`TiposProduto/...` e `TiposValorProduto/...` como áreas top-level)
- [x] 2.7.3 Remover monólito + build/test verde + commit

### 2.8 — DashboardEndpoints (5 rotas, 3 áreas: Dashboard + Aging + PosicaoEstoque)
- [x] 2.8.1 Mapear rotas e separar por capability
- [x] 2.8.2 Gerar pastas (`Dashboard/...`, `Relatorios/Aging/...`, `Relatorios/PosicaoEstoque/...`)
- [x] 2.8.3 Remover monólito + build/test verde + commit

### 2.9 — DividasEndpoints (5 rotas)
- [x] 2.9.1 Mapear rotas
- [x] 2.9.2 Gerar pastas + arquivos
- [x] 2.9.3 Remover monólito + build/test verde + commit

### 2.10 — FornecedoresEndpoints (5 rotas)
- [x] 2.10.1 Mapear rotas
- [x] 2.10.2 Gerar pastas + arquivos
- [x] 2.10.3 Remover monólito + build/test verde + commit

### 2.11 — RolesEndpoints (5 rotas, 2 áreas: Role + Permission)
- [x] 2.11.1 Mapear rotas e separar por entidade
- [x] 2.11.2 Gerar pastas (`Roles/...` e `Permissoes/...` como áreas top-level)
- [x] 2.11.3 Remover monólito + build/test verde + commit

### 2.12 — ClientesEndpoints (6 rotas)
- [x] 2.12.1 Mapear rotas
- [x] 2.12.2 Gerar pastas + arquivos
- [x] 2.12.3 Remover monólito + build/test verde + commit

### 2.13 — EstoqueEndpoints (6 rotas, 2 áreas: Estoque + Inventario)
- [x] 2.13.1 Mapear rotas; atenção a regras de saldo real-time (não mexer em handler)
- [x] 2.13.2 Gerar pastas (`Estoque/...`, `Inventario/...`)
- [x] 2.13.3 Remover monólito + build/test verde + commit

### 2.14 — ProdutosEndpoints (6 rotas)
- [x] 2.14.1 Mapear rotas
- [x] 2.14.2 Gerar pastas + arquivos
- [x] 2.14.3 Remover monólito + build/test verde + commit

### 2.15 — VendasEndpoints (7 rotas, 4 áreas: Orcamento + PedidoVenda + Faturamento + DevolucaoVenda)
- [x] 2.15.1 Mapear rotas e separar por entidade
- [x] 2.15.2 Gerar pastas (`Vendas/Orcamento/...`, `Vendas/PedidoVenda/...`, `Vendas/Faturamento/...`, `Vendas/DevolucaoVenda/...`, `Relatorios/Vendas/...`)
- [x] 2.15.3 Remover monólito + build/test verde + commit

### 2.16 — ComprasEndpoints (10 rotas, 3 áreas: Solicitacao + PedidoCompra + Recebimento) — último, maior
- [x] 2.16.1 Mapear rotas e separar por entidade
- [x] 2.16.2 Gerar pastas (`Compras/SolicitacaoCompra/...`, `Compras/PedidoCompra/...`, `Compras/RecebimentoCompra/...`)
- [x] 2.16.3 Remover monólito + build/test verde + commit

---

## Fase 3 — Retrofit padrão Request/Response/Map nos módulos pré-existentes

> **Contexto**: 10 módulos foram split em refactor anterior usando padrão antigo (Endpoint vincula `Command` direto, `Response.cs`/`Map.cs` apenas comentários). O padrão correto adotado pela Fase 2 (calibrador CentrosDeCusto) é:
> - **Request**: DTO HTTP de entrada
> - **Response**: DTO HTTP de saída
> - **Map**: extension methods `ToCommand/ToQuery` (Request→Command/Query) e `ToResponse` (Result→Response)
>
> Esta fase normaliza os 10 módulos retroativos. Procedimento por endpoint:
> 1. Ler `<Verbo><Recurso>Endpoint.cs` atual
> 2. Identificar campos do Command/Query e Result correspondentes
> 3. Criar `<Verbo><Recurso>Request.cs` espelhando Command/Query (sem campos derivados de rota como Id)
> 4. Criar `<Verbo><Recurso>Response.cs` espelhando Result (record dedicado, não alias)
> 5. Reescrever `<Verbo><Recurso>Map.cs` com `ToCommand`/`ToQuery` + `ToResponse`
> 6. Reescrever `<Verbo><Recurso>Endpoint.cs` para usar Request/Response via Map
> 7. Build + RouteSnapshot verde + commit por módulo

### 3.1 — Auditoria (3 endpoints: ListarLogs, ExportarLogs, HistoricoRegistro)
- [x] 3.1.1 Retrofit ListarLogs (Endpoint+Request+Response+Map)
- [x] 3.1.2 Retrofit ExportarLogs (Endpoint+Request+Response+Map)
- [x] 3.1.3 Retrofit HistoricoRegistro (Endpoint+Request+Response+Map)
- [x] 3.1.4 Build + RouteSnapshot verde + commit

### 3.2 — Auth (4 endpoints: Login, Logout, ConfirmarEmail, RenovarToken)
- [x] 3.2.1 Retrofit Login
- [x] 3.2.2 Retrofit Logout
- [x] 3.2.3 Retrofit ConfirmarEmail
- [x] 3.2.4 Retrofit RenovarToken
- [x] 3.2.5 Build + RouteSnapshot verde + commit

### 3.3 — ConciliacaoBancaria (1 endpoint: ImportarExtrato)
- [x] 3.3.1 Retrofit ImportarExtrato (atenção: provável upload multipart)
- [x] 3.3.2 Build + RouteSnapshot verde + commit

### 3.4 — Despesa (6 endpoints: Criar, Alterar, Excluir, Listar, Obter, Baixar)
- [x] 3.4.1 Retrofit CriarDespesa
- [x] 3.4.2 Retrofit AlterarDespesa
- [x] 3.4.3 Retrofit ExcluirDespesa
- [x] 3.4.4 Retrofit ListarDespesas
- [x] 3.4.5 Retrofit ObterDespesa
- [x] 3.4.6 Retrofit BaixarDespesa
- [x] 3.4.7 Build + RouteSnapshot verde + commit

### 3.5 — Empresas (2 endpoints: Criar, Alterar)
- [x] 3.5.1 Retrofit CriarEmpresa
- [x] 3.5.2 Retrofit AlterarEmpresa
- [x] 3.5.3 Build + RouteSnapshot verde + commit

### 3.6 — FeatureFlags (4 endpoints: Listar, Obter, Alterar, Recarregar)
- [x] 3.6.1 Retrofit ListarFeatureFlags
- [x] 3.6.2 Retrofit ObterFeatureFlag
- [x] 3.6.3 Retrofit AlterarFeatureFlag
- [x] 3.6.4 Retrofit RecarregarFeatureFlags
- [x] 3.6.5 Build + RouteSnapshot verde + commit

### 3.7 — FluxoDeCaixa (2 endpoints: Obter, FecharPeriodo)
- [x] 3.7.1 Retrofit ObterFluxo
- [x] 3.7.2 Retrofit FecharPeriodo
- [x] 3.7.3 Build + RouteSnapshot verde + commit

### 3.8 — Receita (6 endpoints: Criar, Alterar, Excluir, Listar, Obter, Receber)
- [x] 3.8.1 Retrofit CriarReceita
- [x] 3.8.2 Retrofit AlterarReceita
- [x] 3.8.3 Retrofit ExcluirReceita
- [x] 3.8.4 Retrofit ListarReceitas
- [x] 3.8.5 Retrofit ObterReceita
- [x] 3.8.6 Retrofit ReceberReceita
- [x] 3.8.7 Build + RouteSnapshot verde + commit

### 3.9 — Tenants (5 endpoints: Registrar, Listar, Obter, Alterar, Excluir)
- [x] 3.9.1 Retrofit RegistrarTenant
- [x] 3.9.2 Retrofit ListarTenants
- [x] 3.9.3 Retrofit ObterTenant
- [x] 3.9.4 Retrofit AlterarTenant
- [x] 3.9.5 Retrofit ExcluirTenant
- [x] 3.9.6 Build + RouteSnapshot verde + commit

### 3.10 — Usuarios (5 endpoints: Criar, Listar, Obter, Alterar, Excluir)
- [x] 3.10.1 Retrofit CriarUsuario
- [x] 3.10.2 Retrofit ListarUsuarios
- [x] 3.10.3 Retrofit ObterUsuario
- [x] 3.10.4 Retrofit AlterarUsuario
- [x] 3.10.5 Retrofit ExcluirUsuario
- [x] 3.10.6 Build + RouteSnapshot verde + commit

---

## Fase 4 — Limpeza de markers órfãos

- [x] 4.1 Listar todos `*EndpointsResponse.cs` em `Endpoints/V1/` (markers gerados pela Phase 7 do `aderencia-blueprint-acme`)
- [x] 4.2 Listar todos `*EndpointsMap.cs` análogos
- [x] 4.3 Deletar os 16 `*EndpointsResponse.cs` órfãos
- [x] 4.4 Deletar os 16 `*EndpointsMap.cs` órfãos
- [x] 4.5 `dotnet build` verde após limpeza
- [ ] 4.6 Commit `chore(endpoints): remove markers órfãos pós-split`

---

## Fase 5 — Endurecimento do analyzer

- [ ] 5.1 Localizar `ConvencoesBlueprintTests.TodoEndpoint_TemResponseEMap` no projeto Unit Tests
- [ ] 5.2 Substituir iteração por `IEndpoint` types por iteração via `EndpointDataSource.Endpoints`
- [ ] 5.3 Para cada `RouteEndpoint`, resolver arquivo de origem do handler (via `DisplayName` ou metadata) e validar siblings `*Request.cs` + `*Response.cs` + `*Map.cs` na mesma pasta
- [ ] 5.4 Adicionar exceções explícitas (Swagger, Health, etc) em allow-list se necessário
- [ ] 5.5 Test passa para todas as ~120 rotas
- [ ] 5.6 Commit `test(blueprint): endurece analyzer pra iterar EndpointDataSource`

---

## Fase 6 — Documentação

- [ ] 6.1 Atualizar `CLAUDE.md` confirmando que 100% dos endpoints seguem padrão 4-arquivos (Endpoint+Request+Response+Map)
- [ ] 6.2 Remover qualquer nota de "dívida técnica de monoliths" no `CLAUDE.md`
- [ ] 6.3 Atualizar `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md` confirmando padrão Request/Response/Map
- [ ] 6.4 Commit `docs: confirma 100% aderência blueprint pós-split`

---

## Fase 7 — Validação final

- [ ] 7.1 `dotnet build Atena.sln` verde sem warnings novos
- [ ] 7.2 `dotnet test` (unit) verde — incluindo analyzer endurecido
- [ ] 7.3 `dotnet test` (integration HealthCheck + RouteSnapshot) verde
- [ ] 7.4 Validar: 0 arquivos `*Endpoints.cs` em `Endpoints/V1/`
- [ ] 7.5 Validar: ~120 arquivos `*Endpoint.cs` em pastas filhas (mindepth 2)
- [ ] 7.6 `openspec validate split-endpoints-monolitos --strict` verde
