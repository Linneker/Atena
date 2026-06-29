using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IEmpresaRepository : IBaseRepository<Empresa>
{
    Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);

    /// <summary>Primeira empresa ativa do tenant — fallback quando o handler não recebe empresa explícita.</summary>
    Task<Empresa?> GetPrimeiraAtivaAsync(CancellationToken cancellationToken = default);
}
