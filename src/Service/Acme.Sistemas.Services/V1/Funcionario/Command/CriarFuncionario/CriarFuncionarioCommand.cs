using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

public sealed record CriarFuncionarioCommand(
    string NomeCompleto,
    string Cpf,
    string? Email,
    string? Telefone,
    string? Cargo,
    string? Departamento,
    Guid? CentroDeCustoId,
    DateTime? DataAdmissao,
    Guid? UsuarioId) : IRequest<ResponseDefault<CriarFuncionarioCommandResult>>;

public sealed record CriarFuncionarioCommandResult(Guid Id, string NomeCompleto, string Cpf);
