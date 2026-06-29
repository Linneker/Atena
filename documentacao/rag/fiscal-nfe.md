# Fiscal NF-e

## Propósito

Emissão de Nota Fiscal Eletrônica v4.00 (Modelo 55) com **cliente SEFAZ próprio**
(sem dependência de bibliotecas externas de NF-e). Cobre: assinatura ICP-Brasil
A1, SOAP/HTTPS mTLS, contingência SVRS automática, NSR atômico, eventos
(cancelamento, carta correção, inutilização).

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `NFe` | `Domain/Entities/Fiscal/NFe.cs` | Chave 44 dígitos, status, xml in/out, protocolo SEFAZ |
| `NFeItem` | `Domain/Entities/Fiscal/NFeItem.cs` | Produto, CFOP, NCM, CST PIS/COFINS/ICMS, valores |
| `NFeEvento` | `Domain/Entities/Fiscal/NFeEvento.cs` | Cancelamento, CCe, EPEC; vinculado à chave |
| `ConfiguracaoFiscal` | `Domain/Entities/Fiscal/ConfiguracaoFiscal.cs` | Cert A1 PFX criptografado (AES-GCM), ambiente, série, próximo número, regime |
| POCOs XML | `Domain/Entities/Fiscal/Xml/` | Modelos serializáveis para infNFe, det, prot etc. |

## Cliente SEFAZ próprio

Em `Acme.Sistemas.ExternalIntegration/Sefaz/`:

| Componente | Função |
|-----------|--------|
| `RealNFeSefazClient` | Orquestra todo o fluxo (default) |
| `StubNFeSefazClient` | Fallback dev via `Fiscal:UseStub=true` |
| `CertificadoTenantResolver` | Cacheia X509 do tenant até véspera do vencimento |
| `A1CertificadoLoader` | Carrega PFX e valida cadeia ICP-Brasil |
| `XmlSignerC14N` | Assinatura XMLDSig — exc-c14n + RSA-SHA1 (SEFAZ ainda exige SHA-1) |
| `SefazUrlCatalog` | URLs SEFAZ por UF + ambiente (5 UFs prioritárias + SVRS + SVAN) |
| `SefazSoapClient` | SOAP + HTTPS mTLS + Polly retry |
| `ContingenciaPolicy` | Decide SVRS quando UF própria falha consecutivas |
| `NumeradorNFe` | Atômico por `(tenant, cnpj, serie)` — `INSERT…ON DUPLICATE KEY UPDATE LAST_INSERT_ID(col+1)` |
| `XsdValidator` | Valida XML antes de transmitir |

### Serviços SEFAZ implementados (Fase 4)

- `NFeAutorizacaoService` — lote síncrono
- `NFeRetAutorizacaoService` — consulta protocolo retornado
- `NFeConsultaProtocoloService` — consulta por chave
- `NFeStatusServicoService` — health da SEFAZ
- `NFeRecepcaoEventoService` — cancelamento, CCe, manifestação
- `NFeInutilizacaoService` — inutiliza faixa de numeração

## Fluxo de emissão

```
EmitirNfeCommand (vindo de Vendas → Faturamento)
        │
        ▼
NumeradorNFe.ProximoAsync(cnpj, serie) → nNF
        │
        ▼
Monta XML NFe (POCOs → XmlSerializer)
        │
        ▼
XmlSignerC14N.Sign(xml, "NFe<chave>", cert)
        │
        ▼
XsdValidator.Validate(xml)
        │
        ▼
RabbitMQ enfileira → NFeTransmissaoWorker consome
        │
        ▼
RealNFeSefazClient.AutorizarAsync
        ├── ContingenciaPolicy: SVRS se UF própria caiu
        ├── SefazSoapClient envia SOAP via mTLS
        └── Retry exponencial (Polly)
        │
        ▼
Persiste protocolo, status, XML autorizado em S3 (atena-nfe/...)
```

## Storage de XMLs

Layout S3: `{tenant_id}/{ano}/{mes}/{chave}.xml`. Bucket `atena-nfe`. Tanto
XML autorizado quanto inutilização ficam aqui.

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET | `/api/v1/fiscal/nfes` | `nfe:ler` |
| POST | `/api/v1/fiscal/nfes/emitir` | `nfe:emitir` (assíncrono RabbitMQ) |
| POST | `/api/v1/fiscal/nfes/{chave}/cancelar` | `nfe:cancelar` |
| POST | `/api/v1/fiscal/nfes/{chave}/carta-correcao` | `nfe:editar` |
| POST | `/api/v1/fiscal/nfes/inutilizar` | `nfe:editar` |
| GET | `/api/v1/fiscal/nfes/{chave}/xml` | `nfe:ler` |
| GET | `/api/v1/fiscal/nfes/{chave}/danfe.pdf` | `nfe:ler` |
| GET | `/api/v1/fiscal/status-sefaz` | autenticado |
| POST | `/api/v1/fiscal/certificado/importar` | `nfe:editar` |
| GET | `/api/v1/fiscal/configuracao` | `nfe:ler` |
| GET | `/api/v1/fiscal/cfops` | autenticado (catálogo) |
| GET | `/api/v1/fiscal/csts/{tipo}` | autenticado (catálogo) |
| GET | `/api/v1/fiscal/codigos-servico` | autenticado (LC 116) |

## Decisões

- **Cliente próprio** ao invés de NFCom/NFe.Net.Core/etc.: controle total
  da pipeline + reuso para 671 (assinatura) e futuros NFS-e, MDF-e.
- **NumeradorNFe atômico** evita pulos (que exigem Inutilização → multa).
- **SVRS automática** via `ContingenciaPolicy` — N falhas consecutivas
  alternam para SVRS; sucesso volta para UF própria.
- **AES-GCM** criptografa só a senha do PFX; o PFX em si já é protegido pela
  senha PKCS12.
- **SHA-1**, não SHA-256, na assinatura XMLDSig — SEFAZ ainda exige.

## Frontend

- `site/atena-web/src/app/features/fiscal/` — NF-es, certificado, configuração.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Fiscal/`
- `src/Service/Acme.Sistemas.Domain/Interfaces/Fiscal/` (`INumeradorNFe`, `INFeSefazClient`, ...)
- `src/Data/Acme.Sistemas.ExternalIntegration/Sefaz/` (cliente completo)
- `src/Data/Acme.Sistemas.Repository/Repositories/V1/Fiscal/NumeradorNFe.cs`
- `src/Service/Acme.Sistemas.Services/V1/Fiscal/`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/NFeTransmissaoWorker.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/CertificadoVencimentoVarreduraWorker.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Fiscal/`
- Migrations `V20260101017_*` (tabelas NF-e) + `V20260510001_*` (numeração)

## Follow-ups conhecidos

- NFC-e (consumidor final).
- NFS-e (serviços) por município.
- MDF-e (manifesto de transporte).
- DANFE PDF: hoje stub — implementação completa em PR `fiscal-danfe-quest`.
