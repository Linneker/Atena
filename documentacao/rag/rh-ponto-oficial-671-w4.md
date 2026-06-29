# RH Ponto Oficial 671 (W4)

## Propósito

Conformidade com a **Portaria MTP 671/2021** (REP-C cloud). Quando uma empresa
liga `usa_rep_oficial=true`, toda batida do W2/Mobile passa por um subfluxo
que: reserva NSR atômico, gera payload texto anexo II, assina com ICP-Brasil,
persiste `ComprovantePonto`. RH consegue exportar AFD (anexo I) e AEJ (anexo IV).
Sem o W4, o ponto do W2 serve só pra controle interno, sem fé pública.

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Nsr` | `Domain/Entities/Rh/Oficial671/Nsr.cs` | Numerador atômico por (tenant, empresa) — tabela `numerador_nsr` |
| `ComprovantePonto` | `Domain/Entities/Rh/Oficial671/ComprovantePonto.cs` | FK 1:1 com `MarcacaoPonto`, NSR único, payload texto + assinatura Base64 + hash SHA-256 |
| `ConfiguracaoRep` | `Domain/Entities/Rh/Oficial671/ConfiguracaoRep.cs` | Por empresa: tipo (P/C), CNPJ/CEI/CNO, endereço, certificado vinculado, responsável legal |
| `ExportacaoAfd` | `Domain/Entities/Rh/Oficial671/ExportacaoAfd.cs` | Metadados: período, layout versão (003), URL S3, hash, status |
| `ExportacaoAej` | `Domain/Entities/Rh/Oficial671/ExportacaoAej.cs` | Idem + `assinatura_url` (JWS detached) |

Enums: `TipoRep` (RepP / RepC), `StatusExportacao671` (Solicitada / Processando / Concluida / Falhou).

Extensão em `MarcacaoPonto`: campos `Nsr` (long?) + `ComprovanteId` (Guid?).
Extensão em `Empresa`: flag `UsaRepOficial` (bool, default false).

## NSR atômico (Número Sequencial de Registro)

- `INumeradorNsr` em `Domain/Interfaces/Rh/`. Impl `NumeradorNsr` em
  `Repository/V1/Rh/Oficial671/`.
- Idiom MySQL: `INSERT … ON DUPLICATE KEY UPDATE ultimo_numero = LAST_INSERT_ID(ultimo_numero + 1); SELECT LAST_INSERT_ID();`
  — uma única ida ao banco, sem gap entre lock e write. Cópia adaptada do
  `NumeradorNFe`.
- Auditoria diária via `JobAuditarGapsNsrWorker` (compara `count(comprovantes_ponto)`
  vs `numerador_nsr.ultimo_numero`).
- Stress test: `NumeradorNsrConcorrenciaTests` prova 1000 chamadas concorrentes
  → 1000 NSRs únicos contíguos 1..1000.

## Comprovante anexo II

Layout pipe-separated:
```
NSR | TIPO | CPF | PIS | DATA(yyyyMMdd) | HORA(HHmmss) | NOME | CNPJ | HASH_MARCACAO
```

Pipeline (no `EmitirComprovante671`):
1. `INumeradorNsr.ProximoAsync` → NSR
2. `IGeradorComprovantePontoTexto.Gerar(...)` → string
3. `CertificadoTenantResolver.GetAsync` → X509Certificate2 do tenant
4. `IAssinadorComprovante671.Assinar(payload, cert)` → RSA-SHA-256 PKCS#1 v1.5
   sobre UTF-8 bytes → `AssinaturaBase64` + `HashSha256Hex` + `Thumbprint`
5. `IComprovantePontoRepository.AddAsync(comprovante)`
6. `IGeradorComprovantePontoPdf.Gerar` (QuestPDF) gera 1ª via instantânea

## Integração com BaterPonto

`BaterPontoCommandHandler` (W2) agora:
```
1. Cria MarcacaoPonto W2 (sempre)
2. SE Empresa principal do tenant tem UsaRepOficial=true:
   - chama IEmitirComprovante671.EmitirAsync
   - atualiza MarcacaoPonto.Nsr + ComprovanteId
   - Result.Nsr + ComprovanteId + PdfUrl preenchidos
   - falha aqui = log + segue (não falha a batida; gap aparece na auditoria)
3. Retorna BaterPontoCommandResult
```

## AFD — Arquivo Fonte de Dados (anexo I)

Layout texto fixo versão 003. `LayoutAfd003Writer` cobre tipos:
- 1 cabeçalho (CNPJ + CEI + razão + endereço + período + geração)
- 2 identificador REP (versão "ATENA-REP-C")
- 3 marcações (NSR + data + hora + PIS)
- 5 empregados (PIS + CPF + nome)
- 9 trailer (totais + **hash SHA-256 do conteúdo**)

Tipos 4 (ajustes RTC) e 6 (eventos REP) ficam zerados — `rh-671-rtc-eventos`.

## AEJ — Arquivo Eletrônico de Jornada (anexo IV)

JSON v1 com seções `cabecalho`, `jornadas`, `bancosHoras`, `marcacoes`,
`ajustes`, `espelhos`. `GeradorAejV1` serializa via `System.Text.Json` (camelCase).

Assinatura JWS RFC 7515 **detached** (RS256 + `b64=false`) via `AssinadorAej`.
Resultado: `header..signature` (payload vazio entre os pontos) — o cliente
verifica usando o arquivo AEJ separado.

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| POST | `/api/v1/rh/ponto/671/configuracao` | `rh-ponto-oficial:configurar-rep` |
| GET | `/api/v1/rh/ponto/671/configuracao/{empresaId}` | `rh-ponto-oficial:ler` |
| GET | `/api/v1/rh/ponto/671/validar/{empresaId}` | `rh-ponto-oficial:ler` (auto-diagnóstico) |
| GET | `/api/v1/rh/ponto/671/comprovantes/{marcacaoId}.pdf` | `rh-ponto-oficial:emitir-comprovante-2via` |
| POST | `/api/v1/rh/ponto/671/afd/exportar` | `rh-ponto-oficial:exportar-afd` |
| GET | `/api/v1/rh/ponto/671/afd/{exportacaoId}/download` | `rh-ponto-oficial:exportar-afd` |
| POST | `/api/v1/rh/ponto/671/aej/exportar` | `rh-ponto-oficial:exportar-aej` |
| GET | `/api/v1/rh/ponto/671/aej/{exportacaoId}/download?formato={json\|jws}` | `rh-ponto-oficial:exportar-aej` |

## Auto-diagnóstico

`ValidarRepQueryHandler` checa:
1. `ConfiguracaoRep` existe
2. Certificado é carregável (= dentro da validade, senha correta)
3. CNPJ da config aparece no subject do cert (X509 CN tipicamente
   `NOME:CNPJ` para ICP-Brasil)

Retorna `ValidarRepQueryResult { Apto, Checagens[] }` — empresa só deve ativar
`usa_rep_oficial=true` quando todas checagens passarem.

## Frontend

3 telas em `site/atena-web/src/app/features/rh/ponto/oficial-671/`:
- `configuracao-rep.component.ts` — form CRUD
- `auto-diagnostico.component.ts` — chama validar + lista checagens
- `exportar-afd-aej.component.ts` — formulário período + 2 botões + links download

Rotas em `ponto.routes.ts`: `/rh/ponto/671/configuracao`, `/671/diagnostico`,
`/671/exportar`.

Service: `Oficial671Service`.

## Permissões

Recurso: `rh-ponto-oficial`. Ações: `configurar-rep`, `exportar-afd`,
`exportar-aej`, `emitir-comprovante-2via`, mais `Ler` (genérica).

## Migrations (7 do W4)

- `V20260629001_AddTabelaNumeradorNsr`
- `V20260629002_AddTabelaComprovantesPonto`
- `V20260629003_AddTabelaConfiguracaoRep`
- `V20260629004_AddTabelaExportacoesAfd`
- `V20260629005_AddTabelaExportacoesAej`
- `V20260629006_AlterarMarcacoesPontoAdicionarNsr` (`nsr BIGINT NULL` + `comprovante_id CHAR(36) NULL`)
- `V20260629007_AlterarEmpresasAdicionarUsaRepOficial`

## Tests

- `NumeradorNsrConcorrenciaTests` — 1000 chamadas concorrentes via shim in-memory
- `LayoutAfd003WriterTests` — esqueleto sem comprovantes + 3 comprovantes
  ordenados por NSR
- `GeradorComprovantePontoTextoTests` — layout pipe + zero-padding NSR
- `GeradorAejV1Tests` — JSON com todas seções obrigatórias

6/6 testes verde no último build.

## Decisões importantes

- **Reuso do NFe**: `XmlSignerC14N` (primitivas RSA), `CertificadoTenantResolver`
  (mesmo cert do tenant), `NumeradorAtomico` (idiom MySQL).
- **NSR queimado mesmo se emissão falhar** — Portaria 671 proíbe pulos, mas
  uma reserva é sempre consumida; gaps aparecem na auditoria como flag.
- **2ª via determinística**: regenera PDF a partir do `ComprovantePonto`
  persistido → bytes idênticos.
- **AFD/AEJ regenerados on-demand**: o download endpoint re-monta a partir dos
  comprovantes do período (S3 storage é stub no MVP, hash bate).
- **REP-A fora do escopo** (relógio físico hardware autônomo). Só REP-P + REP-C.

## Docs operacionais

`documentacao/rh/ponto-oficial-671.md` — explica fluxo de ativação, NSR,
comprovante, AFD, AEJ, permissões, riscos.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Rh/Oficial671/`
- `src/Service/Acme.Sistemas.Domain/Enums/Rh/{TipoRep,StatusExportacao671}.cs`
- `src/Service/Acme.Sistemas.Domain/Interfaces/Rh/INumeradorNsr.cs`,
  `IAssinadorComprovante671.cs`, `IGeradorComprovantePonto{Texto,Pdf}.cs`
- `src/Service/Acme.Sistemas.Domain/Interfaces/Repository/Rh/I{ConfiguracaoRep,ComprovantePonto,ExportacaoAfd,ExportacaoAej}Repository.cs`
- `src/Data/Acme.Sistemas.Repository/Repositories/V1/Rh/Oficial671/`
- `src/Data/Acme.Sistemas.ExternalIntegration/Rh/Oficial671/AssinadorComprovante671.cs`
- `src/Service/Acme.Sistemas.Services/V1/Rh/Oficial671/` (Servicos, Configuracao, Comprovantes, Afd, Aej)
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Rh/Oficial671/`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/JobAuditarGapsNsrWorker.cs`
- `site/atena-web/src/app/features/rh/ponto/oficial-671/`
- `documentacao/rh/ponto-oficial-671.md`
- `openspec/changes/rh-ponto-oficial-671/`

## Follow-ups conhecidos

- `rh-671-rtc-eventos` — AFD tipos 4 (RTC) e 6 (eventos REP)
- `rh-671-mte-validator-ci` — integrar CLI validador oficial do MTE
- `rh-671-afd-s3-storage` — upload S3 real (hoje URL stub)
- `rh-671-afd-async-worker` / `rh-671-aej-async-worker` — RabbitMQ
- `rh-671-espelho-link-pdf` — botão "2ª via" no espelho do W2
- `rh-671-empresa-toggle` — UI do flag `usa_rep_oficial` na config da empresa
- `rh-671-espelho-marca-dagua` — remover marca d'água quando 671 ativo
- `rh-mobile-comprovante-671` — exibir PDF 671 no app W3
