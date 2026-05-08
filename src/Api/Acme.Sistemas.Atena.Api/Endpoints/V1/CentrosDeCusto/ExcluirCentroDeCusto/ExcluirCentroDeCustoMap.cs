using Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.ExcluirCentroDeCusto;

public static class ExcluirCentroDeCustoMap
{
    public static ExcluirCentroDeCustoCommand ToCommand(this ExcluirCentroDeCustoRequest request)
        => new(request.Id);
}
