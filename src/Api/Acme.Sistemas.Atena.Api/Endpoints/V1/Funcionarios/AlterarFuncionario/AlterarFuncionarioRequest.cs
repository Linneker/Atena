using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.AlterarFuncionario;

public sealed record AlterarFuncionarioRequest(
    string NomeCompleto,
    string? Email,
    string? Telefone,
    string? Cargo,
    string? Departamento,
    Guid? CentroDeCustoId,
    DateTime? DataAdmissao,
    DateTime? DataDemissao,
    Guid? UsuarioId,
    StatusAtivo Status);
