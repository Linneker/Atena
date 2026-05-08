using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

public sealed record ListarContasReceberQuery(
    StatusConta? Status = null,
    DateTime? VencimentoInicio = null,
    DateTime? VencimentoFim = null,
    Guid? ClienteId = null,
    int? DiasAtrasoMinimo = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarContasReceberQueryResult>>;

