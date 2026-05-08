## Why

O Atena foi entregue funcionalmente completo na change `atena-erp-completo`, mas há divergências estruturais em relação ao **blueprint corporativo Acme** (`documentacao/blueprint.yml` + `documentacao/ESTRUTURA_PADRAO_PROJETOS_ACME.md`). O blueprint é norma obrigatória da plataforma Acme: garante que qualquer equipe consiga trabalhar em qualquer projeto sem reaprender layout, convenções de CQRS, organização de endpoints e infraestrutura.

As divergências hoje:

- Endpoints agrupados em **um arquivo por módulo** (`DespesaEndpoints.cs`) em vez de **4 arquivos por verbo** (`CriarDespesa.cs`, `Request`, `Response`, `Map`).
- Commands/Queries têm apenas `Command|Query`, `Handler` e `Validation`. Faltam **`Behavior.cs`** (log/cache/auditoria/enriquecimento) e **`Result.cs`** (contrato de retorno padronizado) — exigidos pelo blueprint.
- `docker-compose.yml` está na raiz; o blueprint exige `infra/compose/docker-compose.yml` e `infra/k8s/kind-config.yaml`.
- Pastas fora do projeto correto: `Core/Messaging`, `Core/Reports`, `Infrastructure/Hosted`, `Infrastructure/Reports`, `Repository/Configuration`.
- Não existe `cache.db` (LiteDB local) no Api, embora o blueprint o exija como **cache distribuído gratuito** (alternativa ao Redis pago), com Redis como provider opcional via feature flag.
- Não existem endpoints de gerenciamento de feature flags em runtime.

A change **não toca o domínio de negócio** — Domain do Atena permanece organizado por módulos ERP (Cadastros, Vendas, Financeiro, Estoque, Compras, Fiscal, Permissions, Tenants, Users, Auditoria), porque cada projeto da plataforma Acme tem seu próprio domínio. O que se compartilha é convenção técnica e infra.

## What Changes

- **Reorganização de infra**: criar `infra/compose/`, mover `docker-compose.yml` para lá, adicionar `infra/k8s/kind-config.yaml` (espelhando AutoProcess), atualizar Dockerfile/CI/scripts.
- **Realocação entre projetos**:
  - `Core/Messaging` → `Infrastructure/Messaging` (consolidar)
  - `Core/Reports` → `Services/V1/Relatorios` (regra de negócio, não infra)
  - `Infrastructure/Hosted` → `Api/Hosted` (workers ficam no Api conforme blueprint)
  - `Infrastructure/Reports` → `Services/V1/Relatorios` (mesmo destino do Core/Reports)
  - `Repository/Configuration/IDataConfiguration.cs` → `Infrastructure/Databases/Configuration/`
- **Cache distribuído gratuito (cache.db)**: implementar `CacheStore` híbrido com LiteDB single-file persistente + camada quente em `IMemoryCache` (TTL 15 min), provider Redis opcional via feature flag. Arquivo `cache.db` no Api raiz, montagem `/tmp/cache.db` em K8s (per-pod, igual AutoProcess).
- **Feature flags em runtime com endpoints REST**: `featureflags.json` carregado com hot-reload via `IOptionsMonitor`; novos endpoints `/api/v1/feature-flags` (GET listar, PUT alternar) protegidos por permissão de admin. Flag `Cache:Provider` com valores `LiteDb`|`Redis`.
- **CQRS quíntuplo**: para **cada** Command e Query existente em `Services/V1/`, adicionar:
  - `*Behavior.cs` — implementação concreta da funcionalidade (log estruturado de entrada/saída, cache lookup com retorno imediato em hit, validações complementares de regra de negócio, enriquecimento de dados).
  - `*Result.cs` — record dedicado de retorno usando `ResponseDefault<T>` quando aplicável.
- **Endpoints 4-arquivos por verbo**: cada verbo HTTP existente vira pasta com `{Verbo}{Recurso}.cs` (`IEndpoint`), `Request.cs` (opcional em GETs simples), `Response.cs`, `Map.cs` (Request↔Command/Query, Result↔Response). **Rotas e contratos HTTP NÃO mudam** — só a organização interna dos arquivos.
- **Behaviors padrão no Core**: `LogBehavior`, `CacheLookupBehavior`, `AuditBehavior`, `ValidationBehavior` registrados uma vez via DI no `DepencieInjection.cs`; cada `*Behavior.cs` por funcionalidade adiciona regras específicas (ex: chave de cache por tenant+id).
- **Convenções endurecidas**: testes-analyzer que falham se um Command/Query não tiver Behavior+Result, ou se um endpoint não seguir o layout 4-arquivos.
- **Documentação**: `CLAUDE.md` atualizado com referência ao blueprint, justificativa de Domain por módulo ERP, e snippet de "como criar novo endpoint" no padrão.

## Capabilities

### New Capabilities

- `feature-flags-runtime`: Gerenciamento de feature flags em runtime via API com hot-reload, escopo por ambiente, endpoints REST de consulta e alternância.
- `cache-distribuido-litedb`: Cache distribuído gratuito (LiteDB + IMemoryCache) com Redis como provider opcional ativável por flag.

### Modified Capabilities

- `refatoracao-arquitetura`: Aderência total ao blueprint Acme — endpoints 4-arquivos, CQRS quíntuplo, layout de pastas, cache híbrido, feature flags.

## Impact

**Estrutura física**
- `infra/compose/docker-compose.yml` (novo path); `infra/k8s/kind-config.yaml` (novo arquivo)
- `Core/Messaging`, `Core/Reports`, `Infrastructure/Hosted`, `Infrastructure/Reports` removidos; conteúdo movido
- `Repository/Configuration/` removido; conteúdo em `Infrastructure/Databases/Configuration/`
- `Api/cache.db` adicionado ao `.gitignore`

**Backend**
- ~80 Commands/Queries ganham `Behavior.cs` + `Result.cs` (volume médio: ~160 arquivos novos)
- ~150 verbos HTTP reorganizados em pastas com 4 arquivos cada (volume alto: ~600 arquivos, mas mecânico)
- Novos endpoints `/api/v1/feature-flags`
- Novo `CacheStore` substitui implementação Redis-only atual

**Banco de dados**
- Sem mudanças de schema. Cache `cache.db` é arquivo local LiteDB.

**API Pública**
- **Sem breaking changes**. Rotas, payloads, status codes e contratos mantidos. Mudança é puramente estrutural.

**Operacional**
- Pods passam a montar `/tmp/cache.db` (volume `emptyDir`); reinício de pod limpa cache local — comportamento igual ao AutoProcess.
- Quando `Cache:Provider=Redis` em produção, LiteDB vira fallback de bootstrap.

**Riscos**
- Volume de mudanças mecânicas é alto; requer disciplina de PRs por funcionalidade para review revisável.
- Behaviors mal escritos podem mascarar bugs (ex: cache stale após escrita) — testes de pipeline são obrigatórios.
- Reorganização de Hosted/Reports cruza projetos; precisa de build verde após cada movimento.
