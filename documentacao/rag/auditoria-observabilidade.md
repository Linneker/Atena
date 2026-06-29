# Auditoria & Observabilidade

## Propósito

Trilha de auditoria de todas as ações sensíveis (commands com `IAuditable`),
log de toda requisição HTTP, hash-chain do ponto (W2) para detectar adulteração,
NLog estruturado para correlação.

## Entidades principais

- `AuditLog` — entrada de auditoria por command (recurso, ação, antes, depois,
  IP, user, timestamp). Path: `Domain/Entities/Auditoria/AuditLog.cs`.
- `ApiRequestAudit` — log de cada request HTTP (rota, método, status, duração,
  user, payload truncado). Path: `Domain/Entities/Auditoria/ApiRequestAudit.cs`.

## Behaviors / Middlewares

- `AuditBehavior` em `Core/Mediators/Behaviors/` — captura antes e depois para
  qualquer `IAuditable` Command. Antes/depois são serializados em JSON.
- `ApiRequestAuditMiddleware` em `Api/Middleware/` — registra cada request.
- `LogBehavior` — adiciona telemetry e structured logging via `ILogger`.

## Marcando um Command como auditável

```csharp
public sealed record CriarFooCommand(Guid Id, string Nome)
    : IRequest<ResponseDefault<...>>, IAuditable
{
    public string Recurso => "Foo";
    public string Acao => "Criar";
}
```

`IAuditable` está em `Domain/Entities/Auditoria/`. Commands sem essa interface
**não** são auditados — decisão deliberada (queries de leitura, jobs internos).

## Hash-chain do ponto (W2)

- Toda `MarcacaoPonto` carrega `hash_anterior` (CHAR(64)) + `hash_integridade` (CHAR(64)).
- `MarcacaoPontoIntegridade.Calcular(funcId, dataHora, tipo, origem, hashAnterior)`
  produz SHA-256 hex (lowercase). Implementação em
  `Services/V1/Rh/Ponto/Engine/MarcacaoPontoIntegridade.cs`.
- Adulterar uma linha quebra a cadeia das seguintes.
- `JobVerificarIntegridadePontoWorker` (24h) varre cadeias por funcionário e
  grava `AuditLog` quando detecta quebra.
- Mobile (W3) replica o mesmo hash em `Mobile.Shared/Helpers/HashHelpers.cs`
  (`CalcularHashBatida` — combina `funcId|timestampUtc|tipo|deviceId`).

## NSR auditoria (W4)

- `JobAuditarGapsNsrWorker` (24h) compara `count(comprovantes_ponto)` vs
  `numerador_nsr.ultimo_numero` por `(tenant, empresa)`. Gap = warning no log.
- Pulos são proibidos pela Portaria 671 — uma reserva é sempre consumida.

## Logging — NLog

- Config em `nlog.config` na raiz da Api.
- Sinks: console (stdout) + arquivo rotativo + filtros por nível.
- Contexto MDC: `tenant_id`, `user_id`, `request_id` automáticos via middleware.

## Endpoints REST de auditoria

| Método | Rota | Permissão | Descrição |
|--------|------|-----------|-----------|
| GET | `/api/v1/auditoria/logs` | `auditoria:ler` | Lista AuditLog paginado |
| GET | `/api/v1/auditoria/requests` | `auditoria:ler` | Lista ApiRequestAudit |

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Core/Mediators/Behaviors/AuditBehavior.cs`
- `src/Service/Acme.Sistemas.Core/Mediators/Behaviors/LogBehavior.cs`
- `src/Service/Acme.Sistemas.Services/V1/Rh/Ponto/Engine/MarcacaoPontoIntegridade.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/JobVerificarIntegridadePontoWorker.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/JobAuditarGapsNsrWorker.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Middleware/ApiRequestAuditMiddleware.cs`
- `test/Unit/Acme.Sistemas.Services.UnitTest/Test/AuditBehaviorTests.cs`

## Follow-ups conhecidos

- Sink para OpenTelemetry / Grafana Tempo (correlation IDs em trace spans).
- Alertas Slack/e-mail quando gap NSR detectado.
- Retenção de logs configurável por tenant.
