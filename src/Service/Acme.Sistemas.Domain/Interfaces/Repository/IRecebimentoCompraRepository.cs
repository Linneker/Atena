using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRecebimentoCompraRepository : IBaseRepository<RecebimentoCompra>
{
    Task<IReadOnlyList<RecebimentoCompra>> ListByPedidoAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RecebimentoCompraItem>> ListItensAsync(Guid recebimentoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<RecebimentoCompraItem> itens, CancellationToken cancellationToken = default);
    Task UpdatePedidoCompraItemQuantidadeRecebidaAsync(Guid pedidoCompraItemId, decimal novaQuantidadeRecebida, CancellationToken cancellationToken = default);
    Task VincularNFeAsync(Guid recebimentoId, string numeroNotaFiscal, string chaveAcesso, CancellationToken cancellationToken = default);
}
