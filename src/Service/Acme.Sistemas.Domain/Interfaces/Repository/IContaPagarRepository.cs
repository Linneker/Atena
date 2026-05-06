using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IContaPagarRepository : IBaseRepository<ContaPagar>
{
    Task<IReadOnlyList<ContaPagar>> ListByFiltroAsync(
        StatusConta? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        Guid? fornecedorId,
        bool somenteVencendoEmAteSeteDias,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<long> CountByFiltroAsync(
        StatusConta? status,
        DateTime? vencimentoInicio,
        DateTime? vencimentoFim,
        Guid? fornecedorId,
        bool somenteVencendoEmAteSeteDias,
        CancellationToken cancellationToken = default);

    Task BaixarAsync(ContaPagar conta, CancellationToken cancellationToken = default);
}
