# RH Ponto Interno (W2)

## Propósito

Sistema de ponto **interno** (sem conformidade Portaria 671 ainda — isso é W4).
Funcionário bate ponto via web, gestor aprova ajustes, sistema gera espelho
mensal, RH fecha competência, banco de horas calcula saldos. Hash-chain
SHA-256 garante integridade do log.

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `MarcacaoPonto` | `Domain/Entities/Rh/MarcacaoPonto.cs` | Tipo (Entrada/SaidaAlmoco/VoltaAlmoco/Saida/Pausa/RetornoPausa), origem (Web/MobileApp/Kiosk/Manual/Importacao), GPS, foto, **hash-chain**, status (Valida/AjusteSolicitado/Ajustada/Invalida). +`Nsr`/`ComprovanteId` quando empresa usa W4. +`ProvaBiometriaLocal`/`TimestampLocal` (W3 mobile). |
| `AjustePonto` | `Domain/Entities/Rh/AjustePonto.cs` | Tipo (AlteracaoHora/Inclusao/Exclusao/Justificativa), marcação original, proposta, motivo, status (Pendente/Aprovado/Rejeitado/Cancelado) |
| `FechamentoPonto` | `Domain/Entities/Rh/FechamentoPonto.cs` | Competência mensal por funcionário; status (Aberto/EmConferencia/Fechado/Reaberto) |
| `BancoHorasPolitica` | `Domain/Entities/Rh/BancoHorasPolitica.cs` | Limites, expiração, conversão; por tenant ou grupo |
| `BancoHorasSaldo` | `Domain/Entities/Rh/BancoHorasSaldo.cs` | Saldo atual por funcionário |
| `MovimentoBancoHoras` | `Domain/Entities/Rh/MovimentoBancoHoras.cs` | Origem (Acumulo/Compensacao/Pagamento/Ajuste/Expiracao), referência ao mês |

## Hash-chain de integridade

`MarcacaoPontoIntegridade.Calcular(funcId, dataHora, tipo, origem, hashAnterior)`
em `Services/V1/Rh/Ponto/Engine/` produz SHA-256 hex (lowercase) determinístico.
Adulterar uma linha quebra cadeia das seguintes. `JobVerificarIntegridadePontoWorker`
(24h) varre e audita. **Hash NÃO substitui ICP-Brasil** — para fé pública use
W4 (671).

## Engine de cálculo

`Services/V1/Rh/Ponto/Engine/` contém:
- `MarcacaoPontoIntegridade` — hash-chain.
- `CalculadoraEspelhoMensal` — dado lista de marcações + jornada vigente +
  política banco-horas, computa por dia: trabalhado, esperado, saldo, atraso,
  anomalias. Retorna `EspelhoMensalDto`.
- `GeradorEspelhoPdf` interface + `GeradorEspelhoPdfQuestPdf` impl (em
  `Services` pra evitar inversão Services→Infrastructure).

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| POST | `/api/v1/rh/ponto/bater` | `rh-ponto:bater-ponto` |
| GET | `/api/v1/rh/ponto/proprio` | `rh-ponto:bater-ponto` (próprias) |
| GET | `/api/v1/rh/ponto/funcionarios/{id}` | `rh-ponto:ler` |
| POST | `/api/v1/rh/ponto/ajustes` | `rh-ponto:ajustar-ponto` |
| GET | `/api/v1/rh/ponto/ajustes/pendentes` | `rh-ponto:aprovar-ponto` |
| POST | `/api/v1/rh/ponto/ajustes/{id}/aprovar` | `rh-ponto:aprovar-ponto` |
| POST | `/api/v1/rh/ponto/ajustes/{id}/rejeitar` | `rh-ponto:aprovar-ponto` |
| GET | `/api/v1/rh/ponto/espelho/{funcId}/{competencia}` | `rh-ponto:ler` |
| GET | `/api/v1/rh/ponto/espelho/{funcId}/{competencia}/pdf` | `rh-ponto:ler` |
| GET/POST | `/api/v1/rh/ponto/fechamentos` | `rh-ponto:*` |
| POST | `/api/v1/rh/ponto/fechamentos/{id}/fechar` | `rh-ponto:fechar-competencia` |
| POST | `/api/v1/rh/ponto/fechamentos/{id}/reabrir` | `rh-ponto:reabrir-competencia` |
| GET | `/api/v1/rh/banco-horas/saldos` | `rh-banco-horas:ler` |
| POST | `/api/v1/rh/banco-horas/movimentos` | `rh-banco-horas:editar` |
| GET/POST/PUT | `/api/v1/rh/banco-horas/politicas` | `rh-politicas-ponto:*` |

Total: **17 endpoints** rh-ponto + rh-banco-horas.

## Frontend

`site/atena-web/src/app/features/rh/ponto/` com 7 telas:
- `meu-ponto/` — funcionário bate ponto + lista próprias batidas hoje
- `espelho/` — calendário mensal com saldos + drill-down dia
- `aprovacoes/` — gestor aprova/rejeita ajustes pendentes
- `banco-horas/` — saldo + extrato de movimentos + políticas
- `fechamento/` — RH fecha/reabre competências
- `oficial-671/` — config + diagnóstico + exportar AFD/AEJ (W4)

## E-mails

3 templates HTML + texto em `documentacao/rh/emails/`:
- `marcacao-suspeita.html` — janela atípica detectada
- `ajuste-aprovado.html`, `ajuste-rejeitado.html` — feedback ao funcionário
- `fechamento-iniciado.html` — alerta RH/gestor

Disparados via fila pelo `EmailDispatcherHostedService`.

## Decisões

- **Status `Valida` ≠ aprovado**: marcação direta do funcionário entra como
  `Valida`; ajustes geram nova `MarcacaoPonto` com status `Ajustada` + a
  original vira `AjusteSolicitado`/`Ajustada`.
- **Hash-chain por funcionário**: cadeias separadas; falha em um funcionário
  não afeta outros.
- **Espelho não é entidade**: é computado on-demand a partir das marcações +
  jornada vigente.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Rh/` (Marcacao, Ajuste, Fechamento, BancoHoras*)
- `src/Service/Acme.Sistemas.Services/V1/Rh/Ponto/`
- `src/Service/Acme.Sistemas.Services/V1/Rh/Ponto/Engine/` (cálculo + integridade + PDF)
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Rh/Ponto/`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/JobVerificarIntegridadePontoWorker.cs`
- `site/atena-web/src/app/features/rh/ponto/`
- `documentacao/rh/manual-operacional-ponto.md`
- `documentacao/rh/emails/`
- Migrations `V20260629001_AddTabelaMarcacoesPonto` + W2 family

## Follow-ups conhecidos

- Reconhecimento facial server-side (hoje só foto/GPS).
- Kiosk app dedicado para fábrica.
- Alertas em tempo real (push web) de batidas suspeitas.
