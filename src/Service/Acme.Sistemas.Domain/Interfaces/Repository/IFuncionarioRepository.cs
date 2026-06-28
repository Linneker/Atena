using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IFuncionarioRepository : IBaseRepository<Funcionario>
{
    Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<Funcionario?> GetByMatriculaAsync(string matricula, CancellationToken cancellationToken = default);
}
