using Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.CriarCentroDeCusto;

public static class CriarCentroDeCustoMap
{
    public static CriarCentroDeCustoCommand ToCommand(this CriarCentroDeCustoRequest request)
        => new(request.Codigo, request.Nome, request.Descricao, request.ResponsavelId);

    public static CriarCentroDeCustoResponse ToResponse(this CriarCentroDeCustoCommandResult result)
        => new(result.Id, result.Codigo, result.Nome);
}
