using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

public sealed class ListarMovimentosQueryHandler
    : IRequestHandler<ListarMovimentosQuery, ResponseDefault<ListarMovimentosQueryResult>>
{
    private readonly IMovimentoBancoHorasRepository _repo;

    public ListarMovimentosQueryHandler(IMovimentoBancoHorasRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarMovimentosQueryResult>> Handle(
        ListarMovimentosQuery request, CancellationToken cancellationToken)
    {
        var movs = await _repo.ListByFuncionarioCompetenciaAsync(
            request.FuncionarioId, request.Competencia, cancellationToken);

        var items = movs
            .Select(m => new ListarMovimentosQueryItem(m.Id, m.Data, m.Origem, m.Minutos, m.Observacao))
            .ToList();
        var saldo = items.Sum(i => i.Minutos);

        return ResponseDefault<ListarMovimentosQueryResult>.Ok(
            new ListarMovimentosQueryResult(items, items.Count, saldo));
    }
}
