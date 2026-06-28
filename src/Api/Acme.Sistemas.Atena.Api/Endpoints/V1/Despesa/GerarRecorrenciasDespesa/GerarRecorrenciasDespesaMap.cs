using Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.GerarRecorrenciasDespesa;

public static class GerarRecorrenciasDespesaMap
{
    public static GerarRecorrenciasDespesaCommand ToCommand(this GerarRecorrenciasDespesaRequest request)
        => new(request.Meses);

    public static GerarRecorrenciasDespesaResponse ToResponse(this GerarRecorrenciasDespesaCommandResult result)
        => new(result.Geradas, result.IgnoradasJaExistentes);
}
