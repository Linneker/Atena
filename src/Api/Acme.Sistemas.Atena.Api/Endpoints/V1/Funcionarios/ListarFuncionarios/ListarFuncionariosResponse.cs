using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.ListarFuncionarios;

public sealed record ListarFuncionariosResponseItem(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    string? Email,
    string? Cargo,
    string? Departamento,
    Guid? CentroDeCustoId,
    DateTime? DataAdmissao,
    DateTime? DataDemissao,
    StatusAtivo Status);

public sealed record ListarFuncionariosResponse(IReadOnlyList<ListarFuncionariosResponseItem> Items);
