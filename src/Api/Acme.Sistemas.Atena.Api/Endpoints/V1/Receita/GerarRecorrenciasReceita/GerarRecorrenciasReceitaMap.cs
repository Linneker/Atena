using Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.GerarRecorrenciasReceita;

public static class GerarRecorrenciasReceitaMap
{
    public static GerarRecorrenciasReceitaCommand ToCommand(this GerarRecorrenciasReceitaRequest request)
        => new(request.Meses);

    public static GerarRecorrenciasReceitaResponse ToResponse(this GerarRecorrenciasReceitaCommandResult result)
        => new(result.Geradas, result.IgnoradasJaExistentes);
}
