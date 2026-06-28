using Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterMobile;

public static class BaterMobileMap
{
    public static BaterPontoMobileCommand ToCommand(this BaterMobileRequest r,
        byte[]? fotoBytes, string? fotoContentType)
        => new(r.Tipo, r.Latitude, r.Longitude, r.DeviceId,
               r.TimestampLocal, r.HashBatida, r.ProvaBiometriaLocal,
               fotoBytes, fotoContentType);

    public static BaterMobileResponse ToResponse(this BaterPontoMobileCommandResult r)
        => new(r.Id, r.DataHora, r.Tipo, r.HashIntegridade, r.FotoUrl);
}
