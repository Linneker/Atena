using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

/// <summary>
/// Helper para a engine de folha (W6): retorna salário vigente de um funcionário em data específica.
/// </summary>
public sealed record ListarSalarioVigenteQuery(Guid FuncionarioId, DateOnly Em)
    : IRequest<ResponseDefault<ListarSalarioVigenteQueryResult>>;
