using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

public sealed record ListarFuncionariosQueryItem(
    Guid Id, string NomeCompleto, string Cpf, string? Email,
    string? Cargo, string? Departamento, Guid? CentroDeCustoId,
    DateTime? DataAdmissao, DateTime? DataDemissao, StatusAtivo Status);

public sealed record ListarFuncionariosQueryResult(IReadOnlyList<ListarFuncionariosQueryItem> Items);
