using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

public sealed record BaterPontoCommand(
    TipoMarcacao? Tipo,
    decimal? Latitude,
    decimal? Longitude,
    string? IpOrigem,
    string? UserAgent,
    string? DeviceId,
    string? FotoUrl) : IRequest<ResponseDefault<BaterPontoCommandResult>>;
