# Infraestrutura

## Propósito

Stack runtime: MySQL para dados, Redis para cache, RabbitMQ para enfileiramento
(NF-e + futuros workers), MinIO/S3 para arquivos (XMLs NF-e, fotos ponto,
AFD/AEJ), Docker Compose para dev, Kubernetes (kind) para staging local.

## Componentes da stack

| Componente | Versão | Uso | Container |
|------------|--------|-----|-----------|
| MySQL | 8.0 | Persistência principal | `mysql:8.0` |
| Redis | 7 | Cache `CacheLookupBehavior` + sessões opcionais | `redis:7-alpine` |
| RabbitMQ | 3.13 mgmt | Filas NF-e (`nfe.transmissao`, `nfe.evento`) | `rabbitmq:3.13-management-alpine` |
| MinIO | latest | S3-compatible — XMLs NF-e, certificados, fotos ponto | `minio/minio` |
| API .NET | net10.0 | App principal | `atena-api` |

## Docker Compose

Path: `infra/compose/docker-compose.yml`. Sobe API + MySQL + Redis + RabbitMQ +
MinIO. Networks isoladas, volumes nomeados para persistência.

```powershell
docker compose -f infra/compose/docker-compose.yml up -d
```

Dockerfile da API em `src/Api/Acme.Sistemas.Atena.Api/Dockerfile` — multi-stage
(SDK build → runtime aspnet). Build context é a raiz do repo (precisa dos
csprojs de todas as camadas).

## Kubernetes (kind)

Manifests em `infra/k8s/v1/`: `namespace.yaml`, `configmap.yaml`, `deployment.yaml`,
`service.yaml`. Cluster local com `kind`: 3 control-plane + 3 worker, port-mapping
`30000→5000`.

```powershell
kind create cluster --name atena --config infra/k8s/kind-config.yaml
kubectl apply -f infra/k8s/v1/
# ou
pwsh infra/k8s/v1/deploy-kind.ps1   # build + load + apply + wait
```

## Hosted Services (Workers em-process)

Registrados em `Program.cs`:

| Worker | Intervalo | Função |
|--------|-----------|--------|
| `PermissionsSeedHostedService` | boot | Seed/sync de `permissions` |
| `NFeTransmissaoWorker` | contínuo (consumer) | Consome `nfe.transmissao` |
| `NFePendenteReprocessadorWorker` | 5min | Reprocessa NF-e em retry |
| `CertificadoVencimentoVarreduraWorker` | 24h | Alerta 30d antes do vencimento |
| `SefazStatusWorker` | 10min | Monitora status da SEFAZ |
| `EmailDispatcherHostedService` | contínuo | Envia e-mails da fila |
| `CacheCleanupWorker` | 1h | Limpa entradas Redis expiradas |
| `RecorrenciaFinanceiraWorker` | diário | Gera ContaPagar/Receber recorrentes |
| `JobVerificarIntegridadePontoWorker` | 24h | Hash-chain ponto W2 |
| `JobAuditarGapsNsrWorker` | 24h | Gaps NSR (671 W4) |
| `DevTenantBootstrapHostedService` | boot (Development) | Cria demo@atena.test |

## Migrations

- Runner próprio em `src/Data/Acme.Sistemas.Infrastructure/Databases/Migrations/`.
- Cada migration implementa `IMigration` com `Version` (long, formato `Vyyyymmddxxx`)
  + `Name` + `Up()` + `Down()`.
- `MigrationRunner` (em boot, antes do host) executa pendentes em ordem de
  `Version`. Helpers úteis: `MigrationHelper.Execute`, `TableExists`, `ColumnExists`.
- Convenção: arquivo `Vyyyymmddxxx_Descricao.cs` em
  `src/Data/Acme.Sistemas.Infrastructure/Databases/Migrations/`.

## Cache (Redis)

- `CacheLookupBehavior` envolve qualquer Query — invalida via
  `ICacheKeyStrategy` por entidade. Comandos `IInvalidaCache` listam recursos
  a derrubar.
- TTL padrão definido em `appsettings.json` por recurso.

## RabbitMQ — filas

- `nfe.transmissao` — payload `{ nfeId, tentativa }` produzido por
  `EmitirNfeCommandHandler`, consumido por `NFeTransmissaoWorker`.
- `nfe.evento` — cancelamento, carta correção.
- Configuração em `appsettings:RabbitMQ` (URI, virtualHost, credenciais).

## MinIO / S3

- Bucket layout NF-e: `s3://atena-nfe/{tenant_id}/{ano}/{mes}/{chave}.xml`.
- Bucket layout ponto foto: `s3://atena-ponto/{tenant_id}/{func_id}/ponto/{aaaamm}/{guid}.jpg`
  (atualmente stub em URL — upload real é PR follow-up).
- Bucket layout AFD: `s3://atena-rh-afd/{tenant}/{empresa}/{periodo}.txt` (stub).
- Bucket layout AEJ: `s3://atena-rh-aej/{tenant}/{empresa}/{periodo}.json` + `.jws`.
- Cliente em `Data/Acme.Sistemas.Infrastructure/GED/` — abstração `IObjectStorage`
  com impl S3-compatible.

## Arquivos para consultar

- `infra/compose/docker-compose.yml`
- `infra/k8s/v1/` (namespace, configmap, deployment, service)
- `infra/k8s/kind-config.yaml`
- `infra/k8s/v1/deploy-kind.ps1`
- `src/Api/Acme.Sistemas.Atena.Api/Dockerfile`
- `src/Api/Acme.Sistemas.Atena.Api/Program.cs` (registra hosted services)
- `src/Data/Acme.Sistemas.Infrastructure/Databases/Migrations/` (200+ migrations)
- `src/Data/Acme.Sistemas.Infrastructure/Databases/Configuration/MigrationRunner.cs`

## Follow-ups conhecidos

- Helm chart oficial (atualmente kustomize-style direto).
- Backup automatizado do MySQL para S3.
- Multi-AZ no K8s de produção.
