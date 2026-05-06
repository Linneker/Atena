using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IDespesaRepository : IBaseRepository<Despesa>
{
    Task<IReadOnlyList<Despesa>> ListByFiltroAsync(
        StatusPagamento? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        string? categoria,
        Guid? competenciaId,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<long> CountByFiltroAsync(
        StatusPagamento? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        string? categoria,
        Guid? competenciaId,
        CancellationToken cancellationToken = default);

    Task BaixarAsync(Despesa despesa, CancellationToken cancellationToken = default);

    Task<decimal> SumByPeriodoAsync(
        DateTime inicio,
        DateTime fim,
        bool somenteBaixadas,
        CancellationToken cancellationToken = default);
}
