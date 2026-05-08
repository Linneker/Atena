## Why

Hoje `INFeSefazClient` é implementado por `StubNFeSefazClient`, que retorna `cStat=100` fake em homologação e bloqueia explicitamente em produção (`cStat=999`). Isso significa que **nenhum cliente real consegue emitir NF-e** com o Atena.

Decisão técnica (2026-05-07): construir o cliente SEFAZ **próprio**, sem depender de `NFe.Net`, `Zeus.Net.NFe`, `Unimake.DFe` etc. Justificativa: independência tecnológica, controle total do código fiscal e auditabilidade. Risco aceito: 5 sprints de esforço focado.

## What Changes

- **Modelo de domínio NF-e v4.00**: classes que mapeiam o XML completo (~800 campos), serializadas com `XmlSerializer` namespace-strict.
- **Geração da chave de acesso**: 44 dígitos com DV mod 11.
- **Validação XSD local**: schemas oficiais embutidos como recursos; validação antes de transmitir.
- **Assinatura digital ICP-Brasil**: XMLDSig com canonicalização C14N exclusive, suporte a A1 (PFX) e A3 (smartcard/token via PKCS#11 ou CSP Windows).
- **Transmissão SOAP/HTTPS com mTLS**: `HttpClient` com `ClientCertificates` configurado por tenant; SOAP 1.2 + WS-Addressing.
- **Catálogo de URLs SEFAZ**: 27 UFs × 2 ambientes × ~6 serviços, embutido como JSON estático versionado.
- **Serviços implementados**: `NFeAutorizacao4`, `NFeRetAutorizacao4`, `NFeConsultaProtocolo4`, `NFeStatusServico4`, `NFeRecepcaoEvento4` (cancel + CC-e), `NFeInutilizacao4`.
- **Contingência SVRS automática**: detecção de indisponibilidade da SEFAZ origem + fallback para SVRS.
- **Numeração sequencial sem pulo**: por (tenant, CNPJ, série) com lock pessimista.
- **Substituição completa do stub**: `StubNFeSefazClient` removido após paridade funcional comprovada.

## Capabilities

### Modified Capabilities

- `fiscal-nfe`: substituir requirements que dependem de stub por requirements de cliente SEFAZ real

### New Capabilities

_(nenhuma — toda a entrega refina a capability existente `fiscal-nfe`)_

## Out of Scope

- NFS-e (serviço, municipal) — ver change `nfse-abrasf-pluggavel`.
- NFC-e (modelo 65) — fica para roadmap futuro.
- MDF-e, CT-e — fora do escopo do MVP fiscal.
- Migração de XMLs de outros sistemas — não aplicável.

## Risks

- **Erro críptico de namespace XML**: SEFAZ rejeita por whitespace/ordering; mitigação: testar contra XSD local antes de transmitir + golden-files de XML reais.
- **Assinatura inválida**: cantonização C14N é tricky; mitigação: validar contra ferramenta externa (e.g., `xmlsec1`) em cada release.
- **A3 (token físico)**: PKCS#11 é dependente de driver do fabricante (SafeNet, Watchdata, Gemalto); mitigação: começar com A1 e adicionar A3 em fase final.
- **SVRS contingência**: lógica de fallback é estado-cheio; bug pode duplicar autorização.
- **Quebra de homologação ao migrar**: clientes que já testaram com stub vão ver comportamento real diferente.
- **Schemas SEFAZ mudam**: layouts NF-e evoluem (4.00 → 4.10 etc); mitigação: versionar schemas como recurso embutido.

## Success Criteria

- Cliente SEFAZ próprio emite NF-e em homologação SP e RJ (mínimo) com `cStat=100` real.
- Cancelamento e CC-e funcionando contra ambiente homolog.
- `StubNFeSefazClient` removido do código de produção.
- Test de integração `Fluxo_Login_PedidoVenda_Faturamento_NFe_DeveCompletar` reativado e verde contra ambiente SEFAZ homologação.
- 5 UFs prioritárias suportadas: SP, RJ, MG, RS, PR.
- Suporte a contingência SVRS testado (simulando indisponibilidade da SEFAZ origem).
- A1 (PFX) totalmente funcional; A3 (PKCS#11) em fase opcional.
