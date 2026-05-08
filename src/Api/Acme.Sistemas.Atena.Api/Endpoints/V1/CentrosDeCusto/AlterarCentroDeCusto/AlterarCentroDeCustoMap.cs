using Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.AlterarCentroDeCusto;

public static class AlterarCentroDeCustoMap
{
    public static AlterarCentroDeCustoCommand ToCommand(this AlterarCentroDeCustoRequest request, Guid id)
        => new(id, request.Nome, request.Descricao, request.ResponsavelId, request.Ativo);

    public static AlterarCentroDeCustoResponse ToResponse(this AlterarCentroDeCustoCommandResult result)
        => new(result.Id);
}
