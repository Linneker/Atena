using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

public sealed class ListarCentrosDeCustoQueryHandler
    : IRequestHandler<ListarCentrosDeCustoQuery, ResponseDefault<ListarCentrosDeCustoQueryResult>>
{
    private readonly ICentroDeCustoRepository _repo;

    public ListarCentrosDeCustoQueryHandler(ICentroDeCustoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarCentrosDeCustoQueryResult>> Handle(ListarCentrosDeCustoQuery request, CancellationToken cancellationToken)
    {
        var centros = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var items = centros.Select(c => new ListarCentrosDeCustoQueryItem(
            c.Id, c.Codigo, c.Nome, c.Descricao, c.ResponsavelId, c.Ativo)).ToList();
        return ResponseDefault<ListarCentrosDeCustoQueryResult>.Ok(
            new ListarCentrosDeCustoQueryResult(items));
    }
}
