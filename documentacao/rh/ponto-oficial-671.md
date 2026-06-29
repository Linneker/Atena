# RH Ponto Oficial — Portaria MTP 671/2021 (W4)

Conformidade do ponto interno do W2 com a Portaria MTP 671/2021: NSR + comprovante
assinado ICP-Brasil + exportação AFD/AEJ. Empresas com `usa_rep_oficial=true` passam
a operar como **REP-C** (Cloud) para fins fiscais.

## Quando uma empresa entra no regime 671

1. Cadastre a `ConfiguracaoRep` em `POST /api/v1/rh/ponto/671/configuracao`
   (frontend: **Configuração REP** em `/rh/ponto/671/configuracao`).
2. Rode o auto-diagnóstico em `GET /api/v1/rh/ponto/671/validar/{empresaId}`
   (frontend: **Auto-diagnóstico** em `/rh/ponto/671/diagnostico`).
   - Confere se a configuração existe, certificado é carregável e CNPJ
     bate com o subject do certificado.
3. Ative o flag `usa_rep_oficial=true` na entidade Empresa (campo `usa_rep_oficial`
   da tabela `empresas`, default `0`).

A partir daí toda batida do W2/Mobile chama o subfluxo 671 e devolve `nsr`,
`comprovanteId` e `pdfUrl` no `BaterPontoCommandResult`.

## NSR (Número Sequencial de Registro)

- Tabela `numerador_nsr` com índice único `(tenant_id, empresa_id)`.
- Implementação MySQL atômica: `INSERT … ON DUPLICATE KEY UPDATE LAST_INSERT_ID(col+1)`
  (mesma mecânica do `NumeradorNFe`).
- Auditoria de gaps: `JobAuditarGapsNsrWorker` roda a cada 24h e loga warning
  quando `count(comprovantes) < ultimo_numero`.
- **Pulos são proibidos** pela Portaria — uma reserva é sempre consumida. Se a
  emissão do comprovante falhar (cert indisponível, etc.), o NSR fica "queimado"
  e aparece como gap na auditoria — investigar.

## Comprovante anexo II

Layout pipe-separated:
```
NSR | TIPO | CPF | PIS | DATA(yyyyMMdd) | HORA(HHmmss) | NOME | CNPJ | HASH_MARCACAO
```

Assinado por `AssinadorComprovante671` (RSA-SHA-256 PKCS#1 v1.5 sobre UTF-8
bytes). Cert vem do `CertificadoTenantResolver` (mesma gestão do NFe).

PDF gerado por `GeradorComprovantePontoPdf` (QuestPDF) — 1 página A4 com header
+ corpo + assinatura/hash + URL de verificação opcional. Determinístico.

**2ª via**: `GET /api/v1/rh/ponto/671/comprovantes/{marcacaoId}.pdf` —
regenera a partir do `ComprovantePonto` persistido. Bytes idênticos.

## AFD — Arquivo Fonte de Dados (anexo I)

Layout texto fixo versão 003. Endpoint:
```
POST /api/v1/rh/ponto/671/afd/exportar
Body: { empresaId, periodoInicio, periodoFim }
```

Resposta inclui `exportacaoId`, `arquivoUrl` (S3 stub) e `hashSha256` do conteúdo
completo. Download em `GET /api/v1/rh/ponto/671/afd/{id}/download` regenera
determinístico a partir dos comprovantes do período (mesmo hash).

Tipos cobertos no MVP: 1 (cabeçalho), 2 (identificador REP), 3 (marcações),
5 (empregados), 9 (trailer). Tipos 4 (RTC) e 6 (eventos REP) são emitidos
zerados — ver `rh-671-rtc-eventos`.

## AEJ — Arquivo Eletrônico de Jornada (anexo IV)

JSON v1 com seções `cabecalho`, `jornadas`, `bancosHoras`, `marcacoes`,
`ajustes`, `espelhos`. Assinatura JWS RFC 7515 **detached** (RS256 + b64=false).

Endpoints:
```
POST /api/v1/rh/ponto/671/aej/exportar  → enfileira/processa síncrono
GET  /api/v1/rh/ponto/671/aej/{id}/download             → JSON
GET  /api/v1/rh/ponto/671/aej/{id}/download?formato=jws → JWS detached
```

## Validação contra validador CLI do MTE

Documentado como follow-up `rh-671-mte-validator-ci`: o MTE publica um
binário CLI gratuito que valida AFD/AEJ contra layout oficial. Plano: rodar
como teste de aceitação no CI quando o binário for disponibilizado em ambiente
container.

Localmente já é possível baixar o validador, gerar AFD via
`POST /671/afd/exportar`, baixar via `GET /671/afd/{id}/download` e rodar
o validador contra o arquivo.

## Permissões

- `rh-ponto-oficial:ler`
- `rh-ponto-oficial:configurar-rep`
- `rh-ponto-oficial:exportar-afd`
- `rh-ponto-oficial:exportar-aej`
- `rh-ponto-oficial:emitir-comprovante-2via`

## Riscos

| Risco | Mitigação atual | Follow-up |
|-------|-----------------|-----------|
| Mudança na Portaria | Layouts versionados (`LayoutAfd003Writer`, `GeradorAejV1`) | `rh-671-layout-v004` |
| Cert A1 vence | Reuso `CertificadoTenantResolver` + `CertificadoVencimentoVarreduraWorker` | n/a |
| NSR perdido / gap | `JobAuditarGapsNsrWorker` diário | Alerta por e-mail/Slack |
| Layout não passa CLI MTE | Unit tests cobrem estrutura | `rh-671-mte-validator-ci` |
| Concorrência batida + comprovante | Numerador atômico MySQL | Stress test 1k concorrentes (`NumeradorNsrConcorrenciaTests`) |
