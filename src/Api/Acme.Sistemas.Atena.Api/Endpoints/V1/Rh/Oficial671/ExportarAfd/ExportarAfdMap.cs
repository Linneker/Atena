using Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAfd;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAfd;

public static class ExportarAfdMap
{
    public static ExportarAfdCommand ToCommand(this ExportarAfdRequest r)
        => new(r.EmpresaId, r.PeriodoInicio, r.PeriodoFim);

    public static ExportarAfdResponse ToResponse(this ExportarAfdCommandResult r)
        => new(r.ExportacaoId, r.Status, r.ArquivoUrl, r.HashSha256);
}
