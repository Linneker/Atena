using Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ExcluirReceita;

public static class ExcluirReceitaMap
{
    public static ExcluirReceitaCommand ToCommand(this ExcluirReceitaRequest request)
        => new(request.Id);
}
