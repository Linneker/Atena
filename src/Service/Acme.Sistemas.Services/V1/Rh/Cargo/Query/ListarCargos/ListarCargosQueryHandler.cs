using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

public sealed class ListarCargosQueryHandler
    : IRequestHandler<ListarCargosQuery, ResponseDefault<ListarCargosQueryResult>>
{
    private readonly ICargoRepository _repo;

    public ListarCargosQueryHandler(ICargoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarCargosQueryResult>> Handle(
        ListarCargosQuery request, CancellationToken cancellationToken)
    {
        var cargos = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = cargos
            .Select(c => new ListarCargosQueryItem(
                c.Id, c.Codigo, c.Descricao, c.CodigoCbo, c.SalarioBaseSugerido, c.Ativo))
            .ToList();

        return ResponseDefault<ListarCargosQueryResult>.Ok(
            new ListarCargosQueryResult(items, total));
    }
}
