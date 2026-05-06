using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface INFeRepository : IBaseRepository<NFe>
{
    Task<IReadOnlyList<NFe>> ListByFiltroAsync(StatusNFe? status, DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusNFe? status, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default);
    Task<long> CountAutorizadasNoMesAsync(int ano, int mes, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NFeItem>> ListItensAsync(Guid nfeId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<NFeItem> itens, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, StatusNFe status, string? codigo, string? motivo, string? protocolo, DateTime? dataAutorizacao, string? chaveAcesso, string? xmlUrl, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NFeEvento>> ListEventosAsync(Guid nfeId, CancellationToken cancellationToken = default);
    Task AddEventoAsync(NFeEvento evento, CancellationToken cancellationToken = default);
}
