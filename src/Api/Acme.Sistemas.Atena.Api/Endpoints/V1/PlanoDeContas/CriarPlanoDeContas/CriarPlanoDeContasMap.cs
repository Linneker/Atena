using Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.CriarPlanoDeContas;

public static class CriarPlanoDeContasMap
{
    public static CriarPlanoDeContasCommand ToCommand(this CriarPlanoDeContasRequest request)
        => new(request.Codigo, request.Nome, request.Tipo, request.PaiId, request.AceitaLancamento);

    public static CriarPlanoDeContasResponse ToResponse(this CriarPlanoDeContasCommandResult result)
        => new(result.Id, result.Codigo, result.Nome, result.Nivel);
}
