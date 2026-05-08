using Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.CriarFuncionario;

public static class CriarFuncionarioMap
{
    public static CriarFuncionarioCommand ToCommand(this CriarFuncionarioRequest request)
        => new(request.NomeCompleto, request.Cpf, request.Email, request.Telefone,
            request.Cargo, request.Departamento, request.CentroDeCustoId, request.DataAdmissao, request.UsuarioId);

    public static CriarFuncionarioResponse ToResponse(this CriarFuncionarioCommandResult result)
        => new(result.Id, result.NomeCompleto, result.Cpf);
}
