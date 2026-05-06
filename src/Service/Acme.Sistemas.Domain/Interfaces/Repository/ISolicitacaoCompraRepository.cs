using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ISolicitacaoCompraRepository : IBaseRepository<SolicitacaoCompra>
{
    Task<SolicitacaoCompra?> GetByNumeroAsync(string numero, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SolicitacaoCompra>> ListByFiltroAsync(
        StatusSolicitacaoCompra? status, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusSolicitacaoCompra? status, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SolicitacaoCompraItem>> ListItensAsync(Guid solicitacaoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<SolicitacaoCompraItem> itens, CancellationToken cancellationToken = default);
    Task RemoveItensAsync(Guid solicitacaoId, CancellationToken cancellationToken = default);

    Task<int> NextNumeroAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, StatusSolicitacaoCompra status, Guid? aprovadoPor, DateTime? aprovadoEm, string? motivoRejeicao, CancellationToken cancellationToken = default);
    Task UpdateStatusOnlyAsync(Guid id, StatusSolicitacaoCompra status, CancellationToken cancellationToken = default);
}
