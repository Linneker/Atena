## ADDED Requirements

### Requirement: Cliente SOAP+mTLS para 3 ambientes eSocial

O sistema SHALL prover `EsocialSoapClient` capaz de chamar serviços `EnviarLoteEventos`, `ConsultarLoteEventos`, `ConsultarReciboEvento`, `ConsultarIdentidadeTrabalhador` nos 3 ambientes (Produção, Restrita, Homologação), com mTLS via certificado ICP-Brasil do empregador (resolvido via `CertificadoTenantResolver` do NFe), e retry exponencial Polly.

#### Scenario: Envio de lote em ambiente Restrita

- **GIVEN** empregador X configurado com cert válido e ambiente Restrita
- **WHEN** sistema chama `EnviarLoteAsync(Restrita, xmlAssinado)`
- **THEN** estabelece mTLS handshake com URL Restrita
- **AND** recebe protocolo
- **AND** persiste em `LoteEnvioEsocial.protocolo`

#### Scenario: Retry em 500

- **GIVEN** servidor eSocial Restrita está intermitente
- **WHEN** primeira tentativa retorna 503
- **THEN** Polly retry: 1s, 4s, 16s
- **AND** se todas falham, marca lote como `falha_temporaria` para retry posterior

### Requirement: Assinatura XMLDSig dos eventos

O sistema SHALL assinar cada evento eSocial individualmente com XMLDSig SHA-256 + RSA enveloped, usando o certificado ICP-Brasil do empregador, **reusando** componentes do `nfe-cliente-sefaz-proprio` (`XmlSignerC14N`).

#### Scenario: Evento assinado tem Signature válido

- **GIVEN** evento S-1000 montado
- **WHEN** `AssinadorEventoEsocial.AssinarAsync(xml, cert)`
- **THEN** XML resultante contém `<Signature>` enveloped
- **AND** verificação contra XSD do eSocial passa
- **AND** verificação criptográfica externa (OpenSSL) passa

### Requirement: NSR atômico por empregador

O sistema SHALL prover `NumeradorNsrEsocial` que retorna NSR único e sequencial por empregador, sem gaps, em concorrência.

#### Scenario: 1000 chamadas concorrentes

- **WHEN** 1000 threads chamam `ProximoAsync(empregadorX)`
- **THEN** retornam 1000 valores distintos sequenciais

### Requirement: Estado da máquina de eventos

Todo `EventoEsocial` SHALL transitar entre os estados EmPreparacao → Assinado → Enviado → (Aceito | Rejeitado) → (Retificado | Excluido).

#### Scenario: Evento rejeitado mantém vinculado a sua correção

- **GIVEN** evento E1 enviado e rejeitado com erro de schema
- **WHEN** RH corrige e cria evento E2
- **THEN** E2 tem `evento_anterior_id = E1.id`
- **AND** E1 permanece com status Rejeitado para histórico

### Requirement: Lotes agrupam até 50 eventos

O sistema SHALL agrupar eventos em estado `Assinado` em lotes de até 50 (limite oficial eSocial) por empregador antes do envio, otimizando throughput.

#### Scenario: 120 eventos viram 3 lotes

- **GIVEN** 120 eventos `Assinado` do empregador X
- **WHEN** worker prepara envio
- **THEN** cria 3 lotes (50 + 50 + 20)
- **AND** envia cada lote em paralelo

### Requirement: Worker consulta lotes Enviado

O sistema SHALL ter `EsocialConsultaWorker` que periodicamente consulta status de lotes em estado `Enviado` e atualiza cada `EventoEsocial` para Aceito ou Rejeitado conforme retorno.

#### Scenario: Consulta retorna eventos processados

- **GIVEN** lote com protocolo P1, 50 eventos Enviado
- **WHEN** worker chama `ConsultarLoteAsync(P1)`
- **AND** retorna 45 Aceito + 5 Rejeitado
- **THEN** 45 eventos viram Aceito + recibo gravado
- **AND** 5 viram Rejeitado com `mensagens_eSocial_json`
