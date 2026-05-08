using Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.AlterarPlanoDeContas;

public static class AlterarPlanoDeContasMap
{
    public static AlterarPlanoDeContasCommand ToCommand(this AlterarPlanoDeContasRequest request, Guid id)
        => new(id, request.Nome, request.AceitaLancamento, request.Ativo);

    public static AlterarPlanoDeContasResponse ToResponse(this AlterarPlanoDeContasCommandResult result)
        => new(result.Id);
}
