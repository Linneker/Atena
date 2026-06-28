using Acme.Sistemas.Services.V1.Despesa.Command.ExcluirDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ExcluirDespesa;

public static class ExcluirDespesaMap
{
    public static ExcluirDespesaCommand ToCommand(this ExcluirDespesaRequest request)
        => new(request.Id);
}
