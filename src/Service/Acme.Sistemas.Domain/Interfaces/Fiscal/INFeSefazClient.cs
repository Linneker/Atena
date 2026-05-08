using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

public sealed record SefazResultado(
    bool Sucesso,
    string Codigo,
    string Motivo,
    string? Protocolo,
    DateTime? DataAutorizacao);

public interface INFeSefazClient
{
    Task<SefazResultado> AutorizarAsync(string xmlAssinado, AmbienteFiscal ambiente, string uf, ModoTransmissao modo, CancellationToken cancellationToken = default);
    Task<SefazResultado> EnviarEventoAsync(string xmlEventoAssinado, AmbienteFiscal ambiente, string uf, CancellationToken cancellationToken = default);
}
