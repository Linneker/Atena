using Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.ExcluirFuncionario;

public static class ExcluirFuncionarioMap
{
    public static ExcluirFuncionarioCommand ToCommand(this ExcluirFuncionarioRequest request)
        => new(request.Id);
}
