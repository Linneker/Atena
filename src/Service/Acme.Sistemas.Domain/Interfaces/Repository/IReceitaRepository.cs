using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IReceitaRepository : IBaseRepository<Receita>
{
    Task<IReadOnlyList<Receita>> ListByFiltroAsync(
        StatusPagamento? status,
        DateTime? recebimentoInicio,
        DateTime? recebimentoFim,
        string? categoria,
        Guid? competenciaId,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<long> CountByFiltroAsync(
        StatusPagamento? status,
        DateTime? recebimentoInicio,
        DateTime? recebimentoFim,
        string? categoria,
        Guid? competenciaId,
        CancellationToken cancellationToken = default);

    Task ReceberAsync(Receita receita, CancellationToken cancellationToken = default);

    Task<decimal> SumByPeriodoAsync(
        DateTime inicio,
        DateTime fim,
        bool somenteRecebidas,
        CancellationToken cancellationToken = default);
}
