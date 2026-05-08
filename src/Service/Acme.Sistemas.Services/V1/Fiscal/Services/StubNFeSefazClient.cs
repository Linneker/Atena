using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.V1.Fiscal.Services;

/// <summary>
/// STUB: simula respostas SEFAZ para fluxo de testes.
/// - Em homologação, sempre retorna 100/Autorizado (cZ=100).
/// - Em produção, retorna 999/StubModoIndisponivel para forçar implementação real antes do go-live.
/// </summary>
public sealed class StubNFeSefazClient : INFeSefazClient
{
    private readonly ILogger<StubNFeSefazClient> _logger;

    public StubNFeSefazClient(ILogger<StubNFeSefazClient> logger)
    {
        _logger = logger;
    }

    public Task<SefazResultado> AutorizarAsync(string xmlAssinado, AmbienteFiscal ambiente, string uf, ModoTransmissao modo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("StubSEFAZ: autorizar ambiente={Ambiente} uf={Uf} modo={Modo} bytes={Bytes}",
            ambiente, uf, modo, xmlAssinado.Length);

        if (ambiente == AmbienteFiscal.Producao)
        {
            return Task.FromResult(new SefazResultado(
                Sucesso: false, Codigo: "999",
                Motivo: "StubNFeSefazClient ativo em ambiente de Produção. Configure cliente SEFAZ real antes do go-live.",
                Protocolo: null, DataAutorizacao: null));
        }

        var protocolo = $"{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        return Task.FromResult(new SefazResultado(
            Sucesso: true, Codigo: "100", Motivo: "Autorizado o uso da NF-e (homologação)",
            Protocolo: protocolo, DataAutorizacao: DateTime.UtcNow));
    }

    public Task<SefazResultado> EnviarEventoAsync(string xmlEventoAssinado, AmbienteFiscal ambiente, string uf, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("StubSEFAZ: evento ambiente={Ambiente} uf={Uf} bytes={Bytes}",
            ambiente, uf, xmlEventoAssinado.Length);

        if (ambiente == AmbienteFiscal.Producao)
        {
            return Task.FromResult(new SefazResultado(false, "999", "Stub em produção.", null, null));
        }

        var protocolo = $"E{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        return Task.FromResult(new SefazResultado(true, "135", "Evento registrado e vinculado a NF-e (homologação)",
            protocolo, DateTime.UtcNow));
    }
}
