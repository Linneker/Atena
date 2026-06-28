using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IBancoHorasPoliticaRepository : IBaseRepository<BancoHorasPolitica>
{
    Task<BancoHorasPolitica?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
}

public interface IBancoHorasSaldoRepository : IBaseRepository<BancoHorasSaldo>
{
    Task<BancoHorasSaldo?> GetByFuncionarioCompetenciaAsync(Guid funcionarioId, string competencia, CancellationToken cancellationToken = default);
}

public interface IMovimentoBancoHorasRepository : IBaseRepository<MovimentoBancoHoras>
{
    Task<IReadOnlyList<MovimentoBancoHoras>> ListByFuncionarioCompetenciaAsync(Guid funcionarioId, string competencia, CancellationToken cancellationToken = default);
}
