using Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.FecharPeriodo;

public static class FecharPeriodoMap
{
    public static FecharPeriodoCommand ToCommand(this FecharPeriodoRequest request)
        => new(request.Ano, request.Mes, request.Observacao);

    public static FecharPeriodoResponse ToResponse(this FecharPeriodoCommandResult result)
        => new(
            result.Id,
            result.Ano,
            result.Mes,
            result.TotalReceitas,
            result.TotalDespesas,
            result.Resultado,
            result.FechadoEm);
}
