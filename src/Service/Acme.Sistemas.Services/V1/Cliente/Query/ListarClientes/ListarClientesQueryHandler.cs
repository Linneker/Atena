using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;

public sealed class ListarClientesQueryHandler
    : IRequestHandler<ListarClientesQuery, ResponseDefault<ListarClientesQueryResult>>
{
    private readonly IClienteRepository _repo;

    public ListarClientesQueryHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarClientesQueryResult>> Handle(ListarClientesQuery request, CancellationToken cancellationToken)
    {
        var clientes = await _repo.ListByFiltroAsync(request.Termo, request.Inadimplente, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Termo, request.Inadimplente, cancellationToken);

        var items = clientes.Select(c => new ListarClientesQueryItem(
            c.Id, c.Tipo, c.Nome, c.NomeFantasia, c.Documento,
            c.Email, c.Telefone, c.Status, c.Inadimplente, c.BloqueadoVendas)).ToList();

        return ResponseDefault<ListarClientesQueryResult>.Ok(
            new ListarClientesQueryResult(items, total));
    }
}
