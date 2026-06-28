using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

public sealed class ListarPedidosVendaQueryHandler
    : IRequestHandler<ListarPedidosVendaQuery, ResponseDefault<ListarPedidosVendaQueryResult>>
{
    private readonly IPedidoVendaRepository _repo;
    private readonly IClienteRepository _clientes;
    private readonly IFuncionarioRepository _funcionarios;

    public ListarPedidosVendaQueryHandler(
        IPedidoVendaRepository repo,
        IClienteRepository clientes,
        IFuncionarioRepository funcionarios)
    {
        _repo = repo;
        _clientes = clientes;
        _funcionarios = funcionarios;
    }

    public async Task<ResponseDefault<ListarPedidosVendaQueryResult>> Handle(
        ListarPedidosVendaQuery request,
        CancellationToken cancellationToken)
    {
        var pedidos = await _repo.ListByFiltroAsync(
            request.Status, request.ClienteId, request.VendedorId,
            request.Inicio, request.Fim,
            request.Skip, request.Take, cancellationToken);

        var total = await _repo.CountByFiltroAsync(
            request.Status, request.ClienteId, request.VendedorId,
            request.Inicio, request.Fim, cancellationToken);

        var clienteIds = pedidos.Select(p => p.ClienteId);
        var nomesCliente = await _clientes.GetNomesByIdsAsync(clienteIds, cancellationToken);

        // Vendedor é Funcionário. Não temos GetNomesByIdsAsync no IFuncionarioRepository;
        // por enquanto deixamos VendedorNome null e exibimos só o id no front (futuro: adicionar batch lookup).
        _ = _funcionarios;

        var items = pedidos.Select(p => new ListarPedidosVendaQueryItem(
            p.Id, p.Numero,
            p.ClienteId,
            nomesCliente.TryGetValue(p.ClienteId, out var nome) ? nome : null,
            p.VendedorId, null,
            p.DataEmissao,
            p.ValorTotal, p.Status)).ToList();

        return ResponseDefault<ListarPedidosVendaQueryResult>.Ok(
            new ListarPedidosVendaQueryResult(items, total, request.Skip, request.Take));
    }
}
