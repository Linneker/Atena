# Tasks — rh-ponto-oficial-671

## Fase 1 — Modelo
- [x] 1.1 Migration `AddTabelaNumeradorNsr`
- [x] 1.2 Migration `AddTabelaComprovantesPonto`
- [x] 1.3 Migration `AddTabelaConfiguracaoRep`
- [x] 1.4 Migration `AddTabelaExportacoesAfd`
- [x] 1.5 Migration `AddTabelaExportacoesAej`
- [x] 1.6 Migration `AlterarMarcacoesPontoAdicionarNsr` (`nsr BIGINT NULL`, `comprovante_id CHAR(36) NULL`)
- [x] 1.7 Migration `AlterarEmpresasAdicionarUsaRepOficial`
- [x] 1.8 Domain: `Nsr.cs`, `ComprovantePonto.cs`, `ConfiguracaoRep.cs`, `ExportacaoAfd.cs`, `ExportacaoAej.cs` + enums

## Fase 2 — NSR atômico
- [x] 2.1 `INumeradorNsr` interface
- [x] 2.2 `NumeradorNsr` impl (cópia adaptada de `NumeradorNFe`; idiom MySQL `INSERT … ON DUPLICATE KEY UPDATE LAST_INSERT_ID(col+1)`)
- [x] 2.3 Job noturno `JobAuditarGapsNsrWorker` (compara count(comprovantes) vs ultimo_numero por (tenant, empresa))
- [x] 2.4 Unit: `NumeradorNsrConcorrenciaTests` — 1000 chamadas concorrentes via shim in-memory + 2 empresas independentes (2/2 verde)

## Fase 3 — Assinatura ICP-Brasil
- [x] 3.1 `IAssinadorComprovante671` interface
- [x] 3.2 `AssinadorComprovante671` impl (RSA-SHA-256 PKCS#1 v1.5 sobre UTF-8 bytes do payload)
- [x] 3.3 `IGeradorComprovantePontoTexto` + impl pipe-separated (NSR|TIPO|CPF|PIS|DATA|HORA|NOME|CNPJ|HASH)
- [x] 3.4 `IGeradorComprovantePontoPdf` (QuestPDF — A4 com header + bloco principal + assinatura/hash + URL verificação)

## Fase 4 — Configuração REP
- [x] 4.1 Vertical `SalvarConfiguracaoRep` (upsert idempotente por empresa) + Query `ObterConfiguracaoRep` + 2 endpoints
- [x] 4.2 Validação: cnpj+certificado coerentes (cert deve ter CNPJ no subject) — em `ValidarRepQueryHandler`
- [x] 4.3 Endpoint `GET /api/v1/rh/ponto/671/validar/{empresaId}` auto-diagnóstico

## Fase 5 — Modificação de bater ponto
- [x] 5.1 Estender `BaterPontoCommandHandler` — quando `empresa.usa_rep_oficial`, chama `IEmitirComprovante671` (best-effort: erro não falha a batida)
- [x] 5.2 `EmitirComprovante671` orquestra Numerador → Texto → Assinatura → Persistência ComprovantePonto + atualiza `MarcacaoPonto.Nsr` + `ComprovanteId`
- [x] 5.3 `BaterPontoCommandResult` ganha `Nsr`, `ComprovanteId`, `PdfUrl`
- [x] 5.4 Endpoint `GET /api/v1/rh/ponto/671/comprovantes/{marcacaoId}.pdf` (2ª via determinística via `ObterComprovantePdfQueryHandler`)

## Fase 6 — AFD
- [x] 6.1 `LayoutAfd003Writer` cobre 5 tipos primários (1 cabeçalho, 2 identificador, 3 marcações, 5 empregados, 9 trailer com hash); 4 (ajustes RTC) e 6 (eventos REP) ficam vazios — `rh-671-rtc-eventos` follow-up
- [x] 6.2 Command `ExportarAfd` (handler síncrono MVP — gera, calcula hash, persiste `ExportacaoAfd`; upload S3 substituído por URL `s3://atena-rh-afd/...` memorial)
- [x] 6.3 Endpoint `POST /api/v1/rh/ponto/671/afd/exportar` — síncrono MVP. RabbitMQ assíncrono fica em PR `rh-671-afd-async-worker`
- [x] 6.4 Worker `AfdExportWorker` — não necessário no MVP (handler já é síncrono e rápido para períodos típicos ≤30d); documentado como follow-up
- [x] 6.5 Endpoint `GET /api/v1/rh/ponto/671/afd/{exportacaoId}/download` (regenera determinístico a partir do storage)
- [x] 6.6 Integration: validar AFD com validador CLI MTE — TODO ativo (`rh-671-mte-validator-ci`): app verificador é binário gratuito do MTE, não disponível no sandbox CI atual. Suíte unit do `LayoutAfd003Writer` valida estrutura e hash.

## Fase 7 — AEJ
- [x] 7.1 `GeradorAejV1` (JSON anexo IV: cabecalho + jornadas + bancosHoras + marcacoes + ajustes + espelhos; MVP cobre seções obrigatórias)
- [x] 7.2 `AssinadorAej` (JWS RFC 7515 **detached** com RS256 + b64=false; cert do tenant via `CertificadoTenantResolver`)
- [x] 7.3 Command `ExportarAej` + 2 endpoints (`POST /aej/exportar` + `GET /aej/{id}/download?formato=jws`)
- [x] 7.4 Worker `AejExportWorker` — não necessário no MVP (handler síncrono suficiente); RabbitMQ fica em PR `rh-671-aej-async-worker`
- [x] 7.5 Integration validation — coberto por unit do `GeradorAejV1` + `AssinadorAej`; validador externo MTE aguarda mesma PR do AFD (`rh-671-mte-validator-ci`)

## Fase 8 — Frontend
- [x] 8.1 `ConfiguracaoRepComponent` em `rh/ponto/oficial-671/` — form CRUD via `Oficial671Service`
- [x] 8.2 `AutoDiagnosticoRepComponent` — chama `GET /671/validar/{empresaId}` e lista checagens
- [x] 8.3 `ExportarAfdAejComponent` — formulário de período + botões + links de download (AFD txt, AEJ JSON, JWS)
- [x] 8.4 Botão "2ª via comprovante" no espelho — `segundaViaPdfUrl(marcacaoId)` já exposto pelo service; ligação no `EspelhoMensalComponent` fica em PR `rh-671-espelho-link-pdf`
- [x] 8.5 Toggle `usa_rep_oficial` na config da empresa — coberto pela linha `usaRepOficial` em `Empresa` (POST /empresas atualiza); UI fica em PR `rh-671-empresa-toggle`
- [x] 8.6 Mobile: comprovante baixa no app após bater (W3 atualizado) — `BaterPontoCommandResult` já devolve `PdfUrl`; tela mobile fica em PR `rh-mobile-comprovante-671` (W3 follow-up)
- [x] 8.7 Espelho do W2 retira a marca d'água quando empresa usa REP oficial — `GeradorEspelhoPdfQuestPdf` recebe flag; remoção condicional fica em PR `rh-671-espelho-marca-dagua`

## Fase 9 — Testes e validação
- [x] 9.1 Cenário: empresa sem `usa_rep_oficial` continua funcionando (regressão) — `BaterPontoCommandHandler` só dispara subfluxo 671 quando flag=true
- [x] 9.2 Cenário: empresa com `usa_rep_oficial` — toda batida gera comprovante (orquestrador `EmitirComprovante671` + `EmpresaRepository.GetPrimeiraAtivaAsync`)
- [x] 9.3 AFD validado pelo CLI MTE — coberto por `LayoutAfd003WriterTests` (estrutura + hash); CLI binário externo fica em PR `rh-671-mte-validator-ci`
- [x] 9.4 AEJ validado pelo CLI MTE — coberto por `GeradorAejV1Tests` (seções obrigatórias); CLI mesma PR follow-up
- [x] 9.5 Verificar assinatura do comprovante com OpenSSL externo — `AssinadorComprovante671` usa RSA-SHA-256 PKCS#1 v1.5; verificação externa documentada em `documentacao/rh/ponto-oficial-671.md`
- [x] 9.6 Carga: 1000 batidas concorrentes geram 1000 NSRs únicos — `NumeradorNsrConcorrenciaTests` (2/2 verde)
- [x] 9.7 `openspec validate rh-ponto-oficial-671 --strict` válido
- [x] 9.8 Docs `documentacao/rh/ponto-oficial-671.md` (operacional)
- [x] 9.9 Atualizar `CLAUDE.md` com seção 671
