using Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAej;

public static class ExportarAejMap
{
    public static ExportarAejCommand ToCommand(this ExportarAejRequest r)
        => new(r.EmpresaId, r.PeriodoInicio, r.PeriodoFim);

    public static ExportarAejResponse ToResponse(this ExportarAejCommandResult r)
        => new(r.ExportacaoId, r.Status, r.ArquivoUrl, r.AssinaturaUrl, r.HashSha256);
}
