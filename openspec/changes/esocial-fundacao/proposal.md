## Why

W11. Bloco D do programa. **eSocial** é o sistema federal que recebe TODA a folha brasileira via XML/SOAP (Receita + Caixa + MTE + INSS). Esta onda constrói a **infraestrutura** de transmissão (cliente SOAP, assinatura XMLDSig, NSR, contingência, retentativa, ambiente Produção/Restrita/Homologação), **reusando maciçamente** os componentes do `nfe-cliente-sefaz-proprio`.

Sem essa onda, as W12/W13/W14 (eventos eSocial) não decolam.

## What Changes

### Componentes — visão geral

```
Acme.Sistemas.ExternalIntegration/Esocial/
├── EsocialSoapClient.cs                ◄── espelho de SefazSoapClient
├── AssinadorEventoEsocial.cs           ◄── reusa XmlSignerC14N
├── CatalogoUrlsEsocial.cs              (Produção, Restrita, Homologação)
├── NumeradorNsrEsocial.cs              ◄── reusa NumeradorAtomico
├── EsocialContingenciaPolicy.cs        ◄── espelho de ContingenciaPolicy
├── Eventos/                            (POCOs base para W12/W13/W14)
│   ├── EventoEsocialBase.cs
│   └── Headers.cs
└── Servicos/
    ├── EnvioLoteEventosAsync.cs
    ├── ConsultaLoteEventos.cs
    ├── ConsultaPorReciboEvento.cs
    └── ConsultaIdentidadeTrabalhador.cs
```

### Modelo

- `EmpregadorEsocial`
  - empresa_id (FK), ambiente (Producao=1, Restrita=2, Homologacao=3)
  - certificado_id (reusa cert do tenant)
  - cnpj_empregador, cei, cno
  - classificacao_tributaria, indicativo_cooperativa
  - simples_nacional BOOL
  - desoneracao_folha BOOL
  - status (`Ativo`, `Suspenso`)

- `LoteEnvioEsocial`
  - tenant_id, empregador_id, ambiente
  - tipo_lote (`Tabelas`, `NaoPeriodicos`, `Periodicos`)
  - quantidade_eventos, data_envio
  - protocolo, recibo, status (`EmPreparacao`, `Enviado`, `Aceito`, `Rejeitado`, `Processando`)
  - resposta_xml, erros_json

- `EventoEsocial`
  - tenant_id, empregador_id, lote_id (FK opcional)
  - tipo_evento ('S-1000', 'S-1005', ... — todos os ~45 tipos)
  - id_evento (ID-XXX gerado pelo nosso lado)
  - nsr (BIGINT — pode coincidir com NSR do ponto se aplicável; padrão diferente)
  - xml_evento (texto)
  - assinatura_xml (texto)
  - hash_sha256
  - status (`EmPreparacao`, `Assinado`, `Enviado`, `Aceito`, `Rejeitado`, `Retificado`, `Excluido`)
  - recibo_eSocial, evento_anterior_id (para retificação)
  - data_geracao, data_envio, data_processamento
  - mensagens_eSocial_json

- `EsocialRecibo`
  - tenant_id, evento_id, recibo, hash_evento
  - retorno_xml

### Catálogo URLs eSocial

3 ambientes:
- Produção: `https://webservices.envio.esocial.gov.br/servicos/empregador/...`
- Restrita: `https://webservices.producaorestrita.esocial.gov.br/...`
- Homologação: `https://webservices.consulta.esocial.gov.br/...`

WSDLs versionados (S-1.2 vigente em 2026).

### Soap client

`EsocialSoapClient`:
- `EnviarLoteAsync(loteId, xmlAssinado)` → retorna protocolo
- `ConsultarLoteAsync(protocolo)` → retorna status + eventos processados
- `ConsultarReciboAsync(recibo)` → retorna status do evento individual
- `ConsultarIdentidadeAsync(cpf)` → valida CPF

mTLS via cert ICP-Brasil do empregador. Polly retry exponencial em 5xx/timeout.

### Estado da máquina (por evento)

```
EmPreparacao ─assinar──► Assinado ─enviar──► Enviado
                                                │
                              ┌─────────────────┴─────────────────┐
                              ▼                                   ▼
                          Aceito                              Rejeitado
                              │                                   │
                              │                            (correção + re-envio)
                              ▼                                   ▼
                              │                              EmPreparacao
                              │
                       Retificado (S-3000)
                              │
                              ▼
                          Excluido
```

Reenvio cria NOVO evento com `evento_anterior_id` apontando para o rejeitado/retificado.

### Contingência

eSocial não tem contingência igual NFe (SEFAZ tem SVRS). Mas:
- Se transmissão falha → retentativa exponencial via RabbitMQ.
- Se rejeição é por erro de schema → fica em estado `Rejeitado` aguardando correção.
- Job de retry de eventos `Enviado` sem retorno após 24h.

### Endpoints

```
POST /api/v1/esocial/empregador                       configurar empregador
GET  /api/v1/esocial/empregador
POST /api/v1/esocial/eventos                          (CRUD genérico — fluxo via W12/W13/W14)
GET  /api/v1/esocial/eventos?tipo=&status=
GET  /api/v1/esocial/eventos/{id}
POST /api/v1/esocial/lotes/preparar                   (agrupa eventos pendentes em lote)
POST /api/v1/esocial/lotes/{id}/enviar
POST /api/v1/esocial/lotes/{id}/consultar
GET  /api/v1/esocial/lotes
GET  /api/v1/esocial/dashboard                        (visão geral status)
```

### Worker

- `EsocialEnvioWorker` (RabbitMQ) — consome `EnviarLoteEsocialMessage`, envia, atualiza status.
- `EsocialConsultaWorker` — consulta status de lotes em estado `Enviado` periodicamente.
- `EsocialRetryWorker` — retenta falhas.

### Permissions

- `Recursos.Esocial` × `Acoes.Configurar, Enviar, Consultar, Excluir`.

## Capabilities

### New Capabilities
- `esocial-transmissao` — Cliente SOAP+mTLS+XMLDSig, NSR, contingência/retry, dashboard de eventos.

### Modified Capabilities
- `multi-tenancy` — eventos eSocial são tenant+empresa-scoped.

## Out of Scope
- Eventos específicos (W12 tabelas, W13 não-periódicos, W14 periódicos).
- Importação de XMLs eSocial gerados por terceiros.
- Web service de consulta pública FGTS (separado).
- Dashboard analítico avançado (W15).

## Risks

- **R1**: XSD eSocial muda por versão (S-1.0, S-1.2, S-1.3...). Mitigação: versionar layouts em `Eventos/V1_2/`, `V1_3/`.
- **R2**: Ambiente Restrita tem dados de teste só (não vai pra produção). Mitigação: documentação clara + flag de ambiente por tenant.
- **R3**: mTLS exige cert ICP-Brasil válido. Reuso de `CertificadoTenantResolver` do NFe.
- **R4**: Tempo de processamento eSocial varia (segundos a horas). Mitigação: consulta assíncrona + retry.

## Success Criteria

- `EsocialSoapClient` envia mensagem de teste para ambiente Restrita com sucesso.
- Assinatura XMLDSig valida (XSD do eSocial).
- NSR único por empregador.
- Estado da máquina de eventos transita corretamente em 5 cenários.
- Dashboard mostra status agregado.
- `openspec validate esocial-fundacao --strict` válido.
