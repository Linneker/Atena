using Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.ExcluirPlanoDeContas;

public static class ExcluirPlanoDeContasMap
{
    public static ExcluirPlanoDeContasCommand ToCommand(this ExcluirPlanoDeContasRequest request)
        => new(request.Id);
}
