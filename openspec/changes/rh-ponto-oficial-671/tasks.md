# Tasks — rh-ponto-oficial-671

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaNumeradorNsr`
- [ ] 1.2 Migration `AddTabelaComprovantesPonto`
- [ ] 1.3 Migration `AddTabelaConfiguracaoRep`
- [ ] 1.4 Migration `AddTabelaExportacoesAfd`
- [ ] 1.5 Migration `AddTabelaExportacoesAej`
- [ ] 1.6 Migration `AlterarMarcacoesPontoAdicionarNsrECargo` (`nsr BIGINT NULL`, `comprovante_id CHAR(36) NULL`)
- [ ] 1.7 Migration `AlterarEmpresasAdicionarUsaRepOficial`
- [ ] 1.8 Domain: `Nsr.cs`, `ComprovantePonto.cs`, `ConfiguracaoRep.cs`, `ExportacaoAfd.cs`, `ExportacaoAej.cs` + enums

## Fase 2 — NSR atômico
- [ ] 2.1 `INumeradorNsr` interface
- [ ] 2.2 `NumeradorNsrService` impl (cópia adaptada de NumeradorNFe)
- [ ] 2.3 Job noturno `JobAuditarGapsNsr` (detecta saltos)
- [ ] 2.4 Unit: 1000 chamadas concorrentes (xUnit + Task.WhenAll)

## Fase 3 — Assinatura ICP-Brasil
- [ ] 3.1 `IAssinadorComprovante671` interface
- [ ] 3.2 `AssinadorComprovante671` impl reusando `XmlSignerC14N` primitivas (RSA-SHA-256)
- [ ] 3.3 `IGeradorComprovantePontoTexto` (monta linha conforme anexo II)
- [ ] 3.4 `IGeradorComprovantePontoPdf` (QuestPDF + QR code)

## Fase 4 — Configuração REP
- [ ] 4.1 CRUD `ConfiguracaoRep` (5 verticals + endpoints)
- [ ] 4.2 Validação: cnpj+certificado coerentes (cert deve ter CNPJ no subject)
- [ ] 4.3 Endpoint `/api/v1/rh/ponto/671/validar` auto-diagnóstico

## Fase 5 — Modificação de bater ponto
- [ ] 5.1 Estender `BaterPontoCommandHandler`: se `empresa.usa_rep_oficial`, chama subfluxo 671
- [ ] 5.2 Subfluxo cria comprovante, persiste, atualiza NSR
- [ ] 5.3 Response da batida inclui `comprovanteId + nsr + pdfUrl`
- [ ] 5.4 Endpoint `GET /comprovantes/{marcacaoId}.pdf` (2ª via)

## Fase 6 — AFD
- [ ] 6.1 `LayoutAfd003Writer` por tipo de registro (7 tipos)
- [ ] 6.2 Command `ExportarAfd` (gera arquivo → upload S3 → persiste ExportacaoAfd)
- [ ] 6.3 Endpoint `POST /afd/exportar` (assíncrono em RabbitMQ)
- [ ] 6.4 Worker `AfdExportWorker`
- [ ] 6.5 Endpoint `GET /afd/{id}/download`
- [ ] 6.6 Integration: validar AFD com validador CLI MTE

## Fase 7 — AEJ
- [ ] 7.1 `GeradorAejV1` (compila JSON com jornadas + bancos + marcações + ajustes)
- [ ] 7.2 `AssinadorAej` (JWS detached)
- [ ] 7.3 Command `ExportarAej` + endpoint
- [ ] 7.4 Worker `AejExportWorker`
- [ ] 7.5 Integration validation

## Fase 8 — Frontend
- [ ] 8.1 Tela "Configuração REP" (CRUD)
- [ ] 8.2 Tela "Auto-diagnóstico REP"
- [ ] 8.3 Tela "Exportar AFD/AEJ" (RH/fiscal)
- [ ] 8.4 Botão "2ª via comprovante" no espelho
- [ ] 8.5 Toggle `usa_rep_oficial` na config da empresa
- [ ] 8.6 Mobile: comprovante baixa no app após bater (W3 atualizado)
- [ ] 8.7 Espelho do W2 retira a marca d'água quando empresa usa REP oficial

## Fase 9 — Testes e validação
- [ ] 9.1 Cenário: empresa sem `usa_rep_oficial` continua funcionando (regressão)
- [ ] 9.2 Cenário: empresa com `usa_rep_oficial` — toda batida gera comprovante
- [ ] 9.3 AFD validado pelo CLI MTE
- [ ] 9.4 AEJ validado pelo CLI MTE
- [ ] 9.5 Verificar assinatura do comprovante com OpenSSL externo
- [ ] 9.6 Carga: 1000 batidas concorrentes geram 1000 NSRs únicos
- [ ] 9.7 `openspec validate rh-ponto-oficial-671 --strict` válido
- [ ] 9.8 Docs `documentacao/rh/ponto-oficial-671.md` (operacional)
- [ ] 9.9 Atualizar `CLAUDE.md` com seção 671
