using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

public sealed record ListarSolicitacoesQuery(
    StatusSolicitacaoCompra? Status = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarSolicitacoesQueryResult>>;

