using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

public sealed class ObterSolicitacaoQueryHandler
    : IRequestHandler<ObterSolicitacaoQuery, ResponseDefault<ObterSolicitacaoQueryResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;

    public ObterSolicitacaoQueryHandler(ISolicitacaoCompraRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterSolicitacaoQueryResult>> Handle(ObterSolicitacaoQuery request, CancellationToken cancellationToken)
    {
        var s = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (s is null)
            return ResponseDefault<ObterSolicitacaoQueryResult>.NotFound("Solicitação não encontrada.");

        var itens = await _repo.ListItensAsync(s.Id, cancellationToken);
        var itensView = itens.Select(i => new SolicitacaoItemView(
            i.Id, i.ProdutoId, i.Quantidade, i.PrecoEstimado, i.Observacao)).ToList();

        return ResponseDefault<ObterSolicitacaoQueryResult>.Ok(new ObterSolicitacaoQueryResult(
            s.Id, s.Numero, s.SolicitanteId, s.Justificativa,
            s.ValorTotal, s.DataSolicitacao,
            s.Status, s.AprovadoPor, s.AprovadoEm,
            s.MotivoRejeicao, itensView));
    }
}
