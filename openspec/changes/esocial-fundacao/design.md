# Design — esocial-fundacao

## Reuso NFe

```
nfe-cliente-sefaz-proprio                       esocial-fundacao
─────────────────────────                       ─────────────────
SefazSoapClient                       ─reuso──► EsocialSoapClient (mesma estrutura, URLs diferentes)
XmlSignerC14N                         ─reuso──► AssinadorEventoEsocial (SHA-256 + RSA)
CertificadoTenantResolver             ─reuso──► (mesmo cert)
ContingenciaPolicy                    ─reuso──► (lógica simplificada — retry only)
NumeradorAtomico                      ─reuso──► NumeradorNsrEsocial
```

## Layout XML genérico (eSocial)

Todo evento tem envelope:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<eSocial xmlns="http://www.esocial.gov.br/schema/evt/...">
  <evtXxx Id="ID-EMPREGADOR-NSR">
    <ideEvento>
      <indRetif>1</indRetif>      <!-- 1=original, 2=retif, 3=exclusão -->
      <nrRecibo>NSR_DO_ANTERIOR</nrRecibo>  <!-- se retif -->
      <tpAmb>2</tpAmb>            <!-- 1=Prod, 2=Restrita, 3=Homolog -->
      <procEmi>1</procEmi>        <!-- 1=app empregador -->
      <verProc>X.Y.Z</verProc>
    </ideEvento>
    <ideEmpregador>
      <tpInsc>1</tpInsc>           <!-- 1=CNPJ, 2=CPF -->
      <nrInsc>00000000000000</nrInsc>
    </ideEmpregador>
    <!-- corpo do evento (varia por tipo) -->
  </evtXxx>
  <Signature xmlns="http://www.w3.org/2000/09/xmldsig#">
    <!-- XMLDSig enveloped -->
  </Signature>
</eSocial>
```

ID-EMPREGADOR-NSR: ex `ID1000000000000001202606171530000000000001`.

## Catálogo URLs

```csharp
public sealed class CatalogoUrlsEsocial
{
    public Uri EnvioLote(Ambiente amb) => amb switch
    {
        Ambiente.Producao    => new("https://webservices.envio.esocial.gov.br/servicos/empregador/enviarloteeventos/WsEnviarLoteEventos.svc"),
        Ambiente.Restrita    => new("https://webservices.producaorestrita.esocial.gov.br/..."),
        Ambiente.Homologacao => new("https://webservices.consulta.esocial.gov.br/...")
    };

    public Uri ConsultaLote(Ambiente amb) => ...;
    public Uri ConsultaIdentidade(Ambiente amb) => ...;
}
```

## EsocialSoapClient — esqueleto

```csharp
public sealed class EsocialSoapClient : IEsocialSoapClient
{
    private readonly HttpClient _http;
    private readonly CatalogoUrlsEsocial _urls;
    private readonly IAsyncPolicy<HttpResponseMessage> _retry;

    public async Task<EnvioLoteResponse> EnviarLoteAsync(Ambiente amb, string xmlAssinado, CancellationToken ct)
    {
        var url = _urls.EnvioLote(amb);
        var envelope = MontaEnvelopeSoap("EnviarLoteEventos", xmlAssinado);
        var resp = await _retry.ExecuteAsync(() => _http.PostAsync(url, new StringContent(envelope, Encoding.UTF8, "text/xml"), ct));
        return ParseRespostaEnvio(await resp.Content.ReadAsStringAsync());
    }

    public async Task<ConsultaLoteResponse> ConsultarLoteAsync(Ambiente amb, string protocolo, CancellationToken ct) { ... }
    public async Task<ConsultaReciboResponse> ConsultarReciboAsync(Ambiente amb, string recibo, CancellationToken ct) { ... }
}
```

mTLS configurado no `HttpClientHandler` via cert do `CertificadoTenantResolver`.

## Estado da máquina (canônica)

Cada `EventoEsocial`:

```
EmPreparacao ─[assinar()]─► Assinado
                              │
                       [enviar(lote)]
                              ▼
                          Enviado
                              │
                  [consultarLote→processou]
                              │
                ┌─────────────┴─────────────┐
                ▼                           ▼
            Aceito                      Rejeitado
                │                           │
       [retifica(S-3000)]          [corrige e re-cria]
                ▼                           ▼
           Retificado                  EmPreparacao
                │
         [exclui via S-3000 com indRetif=3]
                ▼
           Excluido
```

## Lote vs evento individual

eSocial recebe lotes (até 50 eventos no MVP — limite oficial). Cada lote tem 1 protocolo; após processamento, cada evento tem 1 recibo.

```
LoteEnvio
  ├── 1 protocolo
  ├── 50 EventoEsocial
       └── cada um com 1 recibo (após aceito)
```

## Worker pipeline

```
1. Eventos em estado `Assinado` (criados por W12/W13/W14) aguardam.
2. EsocialEnvioWorker pega N eventos do mesmo empregador → monta lote → envia.
3. Recebe protocolo, marca Enviado.
4. EsocialConsultaWorker (timer 30s) consulta lotes Enviado.
5. Atualiza eventos para Aceito/Rejeitado com base no retorno.
```

## Retentativa e contingência

- Retry exponencial 3 tentativas: 1s, 4s, 16s (cobrindo blip de rede).
- Após 3 falhas: marca evento `falha_temporaria`, joga na fila de retry de 1h.
- Job diário: se evento `Enviado` há mais de 24h sem retorno → consulta forçada.

## NSR eSocial

Não é o mesmo do REP 671. eSocial usa NSR próprio sequencial por empregador (24 dígitos:`EmpregadorCNPJ + sequencial`).

```csharp
public sealed class NumeradorNsrEsocial : INumeradorNsrEsocial
{
    // Reusa pattern de NumeradorAtomico (lock + increment)
    public async Task<long> ProximoAsync(Guid empregadorId, CancellationToken ct);
}
```

## Tradeoffs

### Por que tabela `EventoEsocial` polimórfica com xml_evento texto?

45 tipos de evento, cada um com schema próprio. Tabela por tipo seria insano. Persistir XML + tipo enum + status genérico permite W12-W14 adicionarem tipos sem mexer em schema.

### Por que SOAP em vez de REST?

eSocial é SOAP. Não temos escolha. Fica encapsulado no `EsocialSoapClient`.

### Assinatura igual NFe (XMLDSig SHA-256 RSA)?

Sim. eSocial usa o mesmo padrão. `XmlSignerC14N` adaptado é perfeitamente reutilizável.

## Test strategy

- Unit: EsocialSoapClient com mock HTTP — happy + 500 + timeout + retry.
- Unit: AssinadorEventoEsocial verifica XSD do envelope.
- Integration: enviar evento dummy S-1000 em Restrita real → recebe protocolo.
- Integration: consultar lote → recebe status.
- Smoke: ciclo completo S-1000 (envio → aceito) em Restrita.
