using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;

public sealed record ListarOrcamentosQuery(
    StatusOrcamento? Status = null, Guid? ClienteId = null,
    int Skip = 0, int Take = 50) : IRequest<ResponseDefault<ListarOrcamentosQueryResult>>;

