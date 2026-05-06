using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;

public sealed record ListarOrcamentosQuery(
    StatusOrcamento? Status = null, Guid? ClienteId = null,
    int Skip = 0, int Take = 50) : IRequest<ResponseDefault<ListarOrcamentosQueryResult>>;

public sealed record ListarOrcamentosQueryItem(
    Guid Id, string Numero, Guid ClienteId, Guid? VendedorId,
    DateTime DataEmissao, DateTime DataValidade,
    decimal ValorTotal, StatusOrcamento Status);

public sealed record ListarOrcamentosQueryResult(IReadOnlyList<ListarOrcamentosQueryItem> Items, long Total);
