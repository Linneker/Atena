## Why

Atena hoje não emite NF-e de Serviço (NFS-e). Para empresas que prestam serviço (consultoria, software, manutenção, etc.) o ERP é incompleto sem isso. NFS-e é fragmentada: ~5570 municípios brasileiros, ~30 padrões diferentes (ABRASF v2.04, Ginfes, IPM, Betha, NotaCarioca, SP-município...). Cobrir tudo é inviável; cobrir nada deixa o produto sem mercado de serviços.

Decisão: arquitetura **adapter pluggável** + implementação inicial do padrão **ABRASF v2.04** (cobre ~50% dos municípios brasileiros), com SP-município e RJ-NotaCarioca priorizados como próximos pelo peso econômico.

## What Changes

- **Modelo de domínio NFS-e**: entidades genéricas `NFSe`, `NFSeItem`, `NFSeEvento` independentes de padrão municipal.
- **Interface `INFSeMunicipalClient`**: contrato comum para emitir, cancelar, consultar.
- **Factory por município**: `NFSeMunicipalClientFactory.Resolve(codigoIbgeMunicipio)` retorna o adapter correto.
- **Adapter ABRASF v2.04**: cliente para todos os municípios que seguem ABRASF padrão.
- **Catálogo IBGE → padrão**: tabela mapeando código IBGE para `PadraoNFSe` (Abrasf, Ginfes, IPM, NotaCarioca, SP, ...).
- **Storage XML análogo a NF-e**: S3/MinIO com path `nfse/{tenant}/{ano}/{mes}/{numero}.xml`.
- **Endpoint REST**: emissão, cancelamento, consulta, download de XML/PDF.
- **Tabela auxiliar de códigos de serviço LC 116**: lista nacional de serviços (123 códigos), suplementada por códigos municipais.
- **Cancelamento e substituição**: padrão ABRASF v2.04 suporta ambos.
- **Não inclui**: SP-município, NotaCarioca, Ginfes, IPM (próximas changes pluggáveis).

## Capabilities

### New Capabilities

- `fiscal-nfse`: Emissão de NFS-e via adapter pluggável por município, começando com ABRASF v2.04.

### Modified Capabilities

_(nenhuma — capability nova; não conflita com `fiscal-nfe`)_

## Out of Scope

- Padrões municipais não-ABRASF (SP, RJ, Ginfes, IPM, Betha, etc.) — cada um vira sua própria change.
- Padrão nacional DPS (em transição, ainda incipiente em 2026) — change futura `nfse-dps-nacional`.
- NFS-e de pessoa física (autônomos sem CNPJ) — fora do MVP.
- Integração com NFC-e (modelo 65) — fora do escopo fiscal.

## Risks

- **Variação dentro do "padrão" ABRASF**: alguns municípios anunciam ABRASF mas têm tweaks. Mitigação: testar contra 5-10 municípios diferentes antes de declarar suporte.
- **Token/login municipal**: alguns ABRASF exigem usuário/senha + token, outros só certificado. Mitigação: configuração por tenant com credentials criptografadas.
- **Cancelamento varia**: alguns aceitam até 24h, outros 5 dias, outros nunca. Mitigação: feature flag por município.
- **Códigos de serviço**: LC 116 nacional + lista municipal. Mitigação: fallback para LC 116 se município não retornar lista.
- **DPS futuro**: arquitetura precisa permitir migração suave quando DPS for obrigatório.

## Success Criteria

- Adapter ABRASF v2.04 emite NFS-e em pelo menos 3 municípios diferentes em homologação (escolher municípios que efetivamente usam ABRASF puro: ex. Vitória-ES, Florianópolis-SC, Porto Alegre-RS — verificar).
- Cancelamento funciona em pelo menos 2 municípios.
- Catálogo IBGE → padrão preenchido para top 50 municípios mais populosos.
- Endpoint REST `/api/v1/nfse` funcional com docs Swagger.
- Test E2E reativado: faturamento de serviço → NFS-e emitida e armazenada.
- Frontend: tela de configuração NFS-e por tenant + tela de emissão/consulta.
