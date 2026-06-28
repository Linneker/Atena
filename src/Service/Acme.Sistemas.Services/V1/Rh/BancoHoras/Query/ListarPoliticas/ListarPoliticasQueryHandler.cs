using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

public sealed class ListarPoliticasQueryHandler
    : IRequestHandler<ListarPoliticasQuery, ResponseDefault<ListarPoliticasQueryResult>>
{
    private readonly IBancoHorasPoliticaRepository _repo;

    public ListarPoliticasQueryHandler(IBancoHorasPoliticaRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarPoliticasQueryResult>> Handle(
        ListarPoliticasQuery request, CancellationToken cancellationToken)
    {
        var pols = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);
        var items = pols.Select(p => new ListarPoliticasQueryItem(
            p.Id, p.Nome, p.LimiteHorasAcumular,
            p.PrazoCompensacaoDias, p.PermitePagarExcedente, p.Ativo)).ToList();
        return ResponseDefault<ListarPoliticasQueryResult>.Ok(
            new ListarPoliticasQueryResult(items, total));
    }
}
