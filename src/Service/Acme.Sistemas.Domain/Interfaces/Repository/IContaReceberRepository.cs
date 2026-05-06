using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IContaReceberRepository : IBaseRepository<ContaReceber>
{
    Task<IReadOnlyList<ContaReceber>> ListByFiltroAsync(
        StatusConta? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        Guid? clienteId,
        int? diasAtrasoMinimo,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<long> CountByFiltroAsync(
        StatusConta? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        Guid? clienteId,
        int? diasAtrasoMinimo,
        CancellationToken cancellationToken = default);

    Task ReceberAsync(ContaReceber conta, CancellationToken cancellationToken = default);
}
