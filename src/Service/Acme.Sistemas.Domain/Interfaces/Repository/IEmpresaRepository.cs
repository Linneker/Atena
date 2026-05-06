using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IEmpresaRepository : IBaseRepository<Empresa>
{
    Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);
}
