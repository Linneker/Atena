using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.CodigoServico.Query.ListarCodigosServico;

public sealed class ListarCodigosServicoQueryHandler
    : IRequestHandler<ListarCodigosServicoQuery, ResponseDefault<ListarCodigosServicoQueryResult>>
{
    private readonly ICodigoServicoLc116Repository _repo;

    public ListarCodigosServicoQueryHandler(ICodigoServicoLc116Repository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarCodigosServicoQueryResult>> Handle(
        ListarCodigosServicoQuery request, CancellationToken cancellationToken)
    {
        var codigos = await _repo.ListAllAsync(cancellationToken);
        var items = codigos.Select(c => new ListarCodigosServicoQueryItem(c.Codigo, c.Descricao)).ToList();
        return ResponseDefault<ListarCodigosServicoQueryResult>.Ok(
            new ListarCodigosServicoQueryResult(items));
    }
}
