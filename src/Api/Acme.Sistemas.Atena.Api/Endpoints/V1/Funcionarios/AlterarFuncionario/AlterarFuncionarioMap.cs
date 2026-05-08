using Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.AlterarFuncionario;

public static class AlterarFuncionarioMap
{
    public static AlterarFuncionarioCommand ToCommand(this AlterarFuncionarioRequest request, Guid id)
        => new(id, request.NomeCompleto, request.Email, request.Telefone, request.Cargo,
            request.Departamento, request.CentroDeCustoId, request.DataAdmissao, request.DataDemissao,
            request.UsuarioId, request.Status);

    public static AlterarFuncionarioResponse ToResponse(this AlterarFuncionarioCommandResult result)
        => new(result.Id);
}
