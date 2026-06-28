using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

public sealed class ListarLotacoesQueryHandler
    : IRequestHandler<ListarLotacoesQuery, ResponseDefault<ListarLotacoesQueryResult>>
{
    private readonly ILotacaoRepository _repo;

    public ListarLotacoesQueryHandler(ILotacaoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarLotacoesQueryResult>> Handle(
        ListarLotacoesQuery request, CancellationToken cancellationToken)
    {
        var lotacoes = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = lotacoes
            .Select(l => new ListarLotacoesQueryItem(l.Id, l.Nome, l.EmpresaId, l.Cnpj, l.Ativo))
            .ToList();

        return ResponseDefault<ListarLotacoesQueryResult>.Ok(
            new ListarLotacoesQueryResult(items, total));
    }
}
