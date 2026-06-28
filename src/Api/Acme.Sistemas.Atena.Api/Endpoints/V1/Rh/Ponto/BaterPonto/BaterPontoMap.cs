using Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterPonto;

public static class BaterPontoMap
{
    public static BaterPontoCommand ToCommand(this BaterPontoRequest r, string? ip, string? userAgent, string? deviceId)
        => new(r.Tipo, r.Latitude, r.Longitude, ip, userAgent, deviceId, r.FotoUrl);

    public static BaterPontoResponse ToResponse(this BaterPontoCommandResult r)
        => new(r.Id, r.DataHora, r.Tipo, r.HashIntegridade);
}
