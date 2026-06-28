using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

public sealed class ListarDepartamentosQueryHandler
    : IRequestHandler<ListarDepartamentosQuery, ResponseDefault<ListarDepartamentosQueryResult>>
{
    private readonly IDepartamentoRepository _repo;

    public ListarDepartamentosQueryHandler(IDepartamentoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarDepartamentosQueryResult>> Handle(
        ListarDepartamentosQuery request, CancellationToken cancellationToken)
    {
        var deptos = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = deptos
            .Select(d => new ListarDepartamentosQueryItem(d.Id, d.Codigo, d.Nome, d.CentroDeCustoId, d.Ativo))
            .ToList();

        return ResponseDefault<ListarDepartamentosQueryResult>.Ok(
            new ListarDepartamentosQueryResult(items, total));
    }
}
