# Tasks — esocial-fundacao

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaEmpregadoresEsocial`
- [ ] 1.2 Migration `AddTabelaLotesEnvioEsocial`
- [ ] 1.3 Migration `AddTabelaEventosEsocial`
- [ ] 1.4 Migration `AddTabelaRecibosEsocial`
- [ ] 1.5 Migration `AddTabelaNumeradorNsrEsocial`
- [ ] 1.6 Domain + enums (AmbienteEsocial, TipoLoteEsocial, StatusLoteEsocial, StatusEventoEsocial)

## Fase 2 — Reuso e adaptação NFe
- [ ] 2.1 Estudar `SefazSoapClient`, `XmlSignerC14N`, `ContingenciaPolicy` e documentar reuso
- [ ] 2.2 Verificar `CertificadoTenantResolver` aceita uso em outro contexto (NFe + eSocial)

## Fase 3 — Catálogo URLs + Soap client
- [ ] 3.1 `CatalogoUrlsEsocial` com 3 ambientes
- [ ] 3.2 `EsocialSoapClient` + interface
- [ ] 3.3 mTLS configurado no HttpClient
- [ ] 3.4 Retry Polly
- [ ] 3.5 Métodos: `EnviarLoteAsync`, `ConsultarLoteAsync`, `ConsultarReciboAsync`, `ConsultarIdentidadeAsync`

## Fase 4 — Assinatura
- [ ] 4.1 `AssinadorEventoEsocial` (reusa primitivas XmlSignerC14N)
- [ ] 4.2 Validação XSD do envelope
- [ ] 4.3 Tests com XMLs fixture

## Fase 5 — NSR
- [ ] 5.1 `NumeradorNsrEsocial` (lock atômico)
- [ ] 5.2 Tests de concorrência

## Fase 6 — Workers
- [ ] 6.1 `EsocialEnvioWorker` (RabbitMQ consumer)
- [ ] 6.2 `EsocialConsultaWorker` (timer 30s)
- [ ] 6.3 `EsocialRetryWorker` (job hourly)

## Fase 7 — API
- [ ] 7.1 CRUD `EmpregadorEsocial`
- [ ] 7.2 Endpoints de Eventos e Lotes
- [ ] 7.3 Endpoint de dashboard agregado
- [ ] 7.4 Endpoint de auto-diagnóstico (testa cert, conectividade Restrita)

## Fase 8 — Frontend
- [ ] 8.1 Tela "Configurar eSocial" (empregador + ambiente + cert)
- [ ] 8.2 Tela "Dashboard eSocial" (status agregado dos eventos)
- [ ] 8.3 Tela "Eventos eSocial" (lista filtrável + drill-down)
- [ ] 8.4 Tela "Lotes" (lista + ação re-enviar)
- [ ] 8.5 Tela "Auto-diagnóstico"

## Fase 9 — Testes
- [ ] 9.1 Unit: EsocialSoapClient com mock HTTP (happy, 500, timeout, retry)
- [ ] 9.2 Unit: AssinadorEventoEsocial (XSD válido)
- [ ] 9.3 Unit: NumeradorNsr 1000 concorrentes
- [ ] 9.4 Integration: enviar S-1000 dummy em Restrita → protocolo recebido
- [ ] 9.5 Integration: consulta protocolo retorna status
- [ ] 9.6 Smoke: ciclo completo S-1000 → Aceito em Restrita
- [ ] 9.7 `openspec validate esocial-fundacao --strict` válido
- [ ] 9.8 Docs `documentacao/rh/esocial-fundacao.md` (operacional + troubleshooting)
