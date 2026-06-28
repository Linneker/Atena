using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IMarcacaoPontoRepository : IBaseRepository<MarcacaoPonto>
{
    Task<MarcacaoPonto?> GetUltimaPorFuncionarioAsync(Guid funcionarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MarcacaoPonto>> ListByFuncionarioPeriodoAsync(
        Guid funcionarioId, DateOnly inicio, DateOnly fim, CancellationToken cancellationToken = default);

    /// <summary>Para o job de verificação de integridade noturno.</summary>
    Task<IReadOnlyList<MarcacaoPonto>> ListAllByFuncionarioOrdenadasAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default);

    /// <summary>Lista todos os funcionarioIds distintos do tenant que tenham ao menos uma marcação.</summary>
    Task<IReadOnlyList<Guid>> ListFuncionarioIdsComMarcacoesAsync(CancellationToken cancellationToken = default);
}
