using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;

public sealed record AlterarFuncionarioCommand(
    Guid Id,
    string NomeCompleto,
    string? Email,
    string? Telefone,
    string? Cargo,
    string? Departamento,
    Guid? CentroDeCustoId,
    DateTime? DataAdmissao,
    DateTime? DataDemissao,
    Guid? UsuarioId,
    StatusAtivo Status) : IRequest<ResponseDefault<AlterarFuncionarioCommandResult>>;

public sealed record AlterarFuncionarioCommandResult(Guid Id);
