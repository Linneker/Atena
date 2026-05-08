# Mapa de dependências — baseline

## Tasks 0.2.1, 0.2.2, 0.2.3

### Grafo atual (final, após correção da Fase 0)

```
Acme.Sistemas.Domain        →  (nenhuma)
Acme.Sistemas.Core          →  (nenhuma)
Acme.Sistemas.ExternalIntegration  →  Core
Acme.Sistemas.Services      →  Core, Domain, ExternalIntegration
Acme.Sistemas.Infrastructure →  Core, Domain                    ← ✓ não depende mais de Services
Acme.Sistemas.Repository    →  Core, Domain, Infrastructure     ← ✓ Repository acessa Infrastructure
Acme.Sistemas.Atena.Api     →  Core, Domain, Services, Repository,
                                Infrastructure, ExternalIntegration
```

> Direção alvo do blueprint atingida: Infrastructure é a camada técnica baixa; Repository (execução de SQL) e Services (regra de negócio) ficam acima. Workers (`IHostedService`) movidos para `Api/Hosted` per blueprint.

### Direção alvo do blueprint (design.md)

```
Repository →  Infrastructure   (Repository depende de Infrastructure;
                                Infrastructure NÃO depende de Repository)
Services   →  Domain, Core, ExternalIntegration
Api        →  Services, Repository, Infrastructure, Core, Domain
```

### Divergências detectadas

#### ~~Divergência 1 — Infrastructure → Repository~~ (FALSA — já correto)
- **Estado real**: edge não existe. `IDataConfiguration` já vive em `Acme.Sistemas.Infrastructure.Databases.Configuration`. Repository hoje NÃO depende de Infrastructure (ainda) e Infrastructure NÃO depende de Repository.
- **Implicação para Fase 2.1**: parte do trabalho já foi feito durante `atena-erp-completo`. O que falta:
  - Adicionar `<ProjectReference>` `Acme.Sistemas.Repository → Acme.Sistemas.Infrastructure` para que repos usem `IDataConfiguration` diretamente do namespace correto (sem o using stale tipo o que estava em `BaseRepositoryTenantFilterTests.cs`).
  - Remover pasta vazia `Repository/Configuration/` (se existir).
  - Atualizar todos os usings em código de produção que ainda apontem para `Acme.Sistemas.Repository.Configuration`.

#### ~~Divergência 2 — Infrastructure → Services~~ (RESOLVIDO na Fase 0)
- **Estado anterior**: edge `Infrastructure → Services` presente; usings dispersos consumindo types de `Services.V1.Fiscal.Services` e `Services.V1.Relatorios.Pdf`
- **Resolução aplicada**:
  - Movidas 4 interfaces NFe (`INFeSefazClient`, `INFeTransmissaoEnqueuer`, `INFeXmlBuilder`, `INFeXmlSigner`) de `Services/V1/Fiscal/Services/` para `Domain/Interfaces/Fiscal/`
  - Movidas 4 interfaces de Reports (`IRelatorioExporter`, `IRelatorioPdfRenderer`, `IDanfePdfRenderer`, `IPedidoCompraPdfRenderer`) + DTOs (`TabelaExport`, `TenantBranding`, `DanfeData`, `PedidoCompraPdfData`) para `Domain/Interfaces/Reports/`
  - Movidos DTOs `BalancoResult`/`BalancoLinha`/`DREResult`/`DRELinha` para `Domain/Reports/`
  - Workers (`NFeTransmissaoWorker`, `CertificadoVencimentoVarreduraWorker`) movidos de `Infrastructure/Hosted` para `Api/Hosted` per blueprint
  - DI registrations Service-side movidas de `InfrastructureServiceCollectionExtensions` para `ServicesServiceCollection`
  - Removida `<ProjectReference>` `Infrastructure → Services`

### Dependências de pacotes externos (sintetizado)

- `Pomelo.EntityFrameworkCore.MySql` em Infrastructure
- `RabbitMQ.Client` em Infrastructure
- `StackExchange.Redis` em Infrastructure (será revisado na Fase 4 — virar opt-in via flag)
- `AWSSDK.S3` em Infrastructure (Ged)
- `LiteDB` — **a adicionar** em Infrastructure na Parte 4.1.1

### Ciclos

- Nenhum ciclo direto detectado pelo `dotnet build` (ele detectaria e falharia). A inversão atual `Infrastructure → Repository` não cria ciclo porque `Repository → Infrastructure` ainda não existe.
- Após Fase 2.1, **a inversão precisa ser feita atomicamente** (remover edge + adicionar edge inversa em uma única operação) para não deixar o build vermelho.

### Recomendações para o resto da change

1. Parte 2.1 deve ser **uma transação**: remover `Infrastructure → Repository`, adicionar `Repository → Infrastructure`, mover `IDataConfiguration.cs`, atualizar usings — tudo num commit.
2. Parte 2.3 e 2.4 **podem reduzir o conjunto de tipos** que Infrastructure consome de Repository, facilitando a flip da Parte 2.1. Considerar reordenar: 2.3, 2.4 antes de 2.1.
3. **Sugestão de ajuste de plano**: trocar a ordem das Partes da Fase 2 para:
   - 2.1 (atual) → Mover `Infrastructure/Hosted` → `Api/Hosted` (era 2.3)
   - 2.2 (atual) → Mover `Infrastructure/Reports` + `Core/Reports` → Services (era 2.4)
   - 2.3 (atual) → Mover `Core/Messaging` → `Infrastructure/Messaging` (era 2.2)
   - 2.4 (atual) → Flip `IDataConfiguration` (era 2.1) — última, depois que Hosted e Reports já saíram
   - Razão: minimiza o conjunto de símbolos que Infrastructure exporta para Repository, simplificando a flip.
