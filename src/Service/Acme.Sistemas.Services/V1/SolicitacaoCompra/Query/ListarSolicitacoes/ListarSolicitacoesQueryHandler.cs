using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

public sealed class ListarSolicitacoesQueryHandler
    : IRequestHandler<ListarSolicitacoesQuery, ResponseDefault<ListarSolicitacoesQueryResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;

    public ListarSolicitacoesQueryHandler(ISolicitacaoCompraRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarSolicitacoesQueryResult>> Handle(ListarSolicitacoesQuery request, CancellationToken cancellationToken)
    {
        var sols = await _repo.ListByFiltroAsync(request.Status, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Status, cancellationToken);

        var items = sols.Select(s => new ListarSolicitacoesQueryItem(
            s.Id, s.Numero, s.SolicitanteId, s.ValorTotal,
            s.DataSolicitacao, s.Status)).ToList();

        return ResponseDefault<ListarSolicitacoesQueryResult>.Ok(
            new ListarSolicitacoesQueryResult(items, total));
    }
}
