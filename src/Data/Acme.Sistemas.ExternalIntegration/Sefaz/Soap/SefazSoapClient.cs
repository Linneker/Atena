using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Soap;

/// <summary>
/// Cliente SOAP/HTTPS para SEFAZ com mTLS (cert do tenant), TLS 1.2+, timeout configurável,
/// retry com backoff exponencial em erros de rede (não em respostas 4xx fiscais), e logging
/// estruturado por requisição.
///
/// Não cacheia HttpClient — o cert por tenant é injetado por requisição via handler descartável.
/// Custo de criação é amortizado pelas latências SEFAZ (centenas de ms).
/// </summary>
public sealed class SefazSoapClient
{
    private readonly SefazUrlCatalog _catalog;
    private readonly ILogger<SefazSoapClient> _logger;
    private readonly TimeSpan _timeout;

    private static readonly ResiliencePipeline<HttpResponseMessage> _retry =
        new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .Handle<TaskCanceledException>()
                    .HandleResult(r => (int)r.StatusCode >= 500),
                MaxRetryAttempts = 2,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromMilliseconds(500),
            })
            .Build();

    public SefazSoapClient(SefazUrlCatalog catalog, ILogger<SefazSoapClient> logger, TimeSpan? timeout = null)
    {
        _catalog = catalog;
        _logger = logger;
        _timeout = timeout ?? TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// Resultado de uma chamada SOAP — corpo `nfeResultMsg` (sem envelope) e metadados.
    /// </summary>
    public sealed record SoapResult(
        bool Sucesso,
        HttpStatusCode StatusCode,
        string? ResultMsg,
        string? ErroMensagem,
        TimeSpan Latencia);

    /// <summary>
    /// Envia o `payloadXml` (já assinado quando aplicável) ao serviço SEFAZ correspondente.
    /// </summary>
    public async Task<SoapResult> EnviarAsync(
        string uf,
        AmbienteFiscal ambiente,
        SefazServico servico,
        string payloadXml,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        var url = _catalog.Resolver(uf, ambiente, servico);
        var (wsdlNs, action) = SoapAction.For(servico);
        var envelope = SoapEnvelopeBuilder.Build(payloadXml, wsdlNs);

        using var handler = BuildHandler(cert);
        using var http = new HttpClient(handler, disposeHandler: false) { Timeout = _timeout };

        var sw = Stopwatch.StartNew();
        HttpResponseMessage? response = null;
        try
        {
            response = await _retry.ExecuteAsync(async ct =>
            {
                using var req = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(envelope, Encoding.UTF8),
                };
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/soap+xml")
                {
                    CharSet = "utf-8",
                    Parameters = { new NameValueHeaderValue("action", $"\"{action}\"") },
                };
                return await http.SendAsync(req, ct).ConfigureAwait(false);
            }, cancellationToken).ConfigureAwait(false);

            sw.Stop();
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var resultMsg = SoapEnvelopeBuilder.ExtractResultMsg(body);

            _logger.LogInformation(
                "SEFAZ {Servico} {UF}/{Ambiente} → {Status} em {Latencia}ms",
                servico, uf, ambiente, (int)response.StatusCode, sw.ElapsedMilliseconds);

            return new SoapResult(
                Sucesso: response.IsSuccessStatusCode && resultMsg is not null,
                StatusCode: response.StatusCode,
                ResultMsg: resultMsg,
                ErroMensagem: response.IsSuccessStatusCode ? null : body,
                Latencia: sw.Elapsed);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogWarning(ex,
                "SEFAZ {Servico} {UF}/{Ambiente} falhou após {Latencia}ms: {Mensagem}",
                servico, uf, ambiente, sw.ElapsedMilliseconds, ex.Message);
            return new SoapResult(false, 0, null, ex.Message, sw.Elapsed);
        }
        finally
        {
            response?.Dispose();
        }
    }

    private static SocketsHttpHandler BuildHandler(X509Certificate2 cert)
    {
        var handler = new SocketsHttpHandler
        {
            SslOptions = new SslClientAuthenticationOptions
            {
                ClientCertificates = new X509CertificateCollection { cert },
                EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                LocalCertificateSelectionCallback = (_, _, _, _, _) => cert,
            },
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        };
        return handler;
    }
}
