using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

/// <summary>
/// Consultas cross-tenant — usadas por workers de varredura.
/// Não respeita TenantContext; deve ser usada apenas em contextos administrativos.
/// </summary>
public interface IConfiguracoesFiscaisQueryRepository
{
    Task<IReadOnlyList<ConfiguracaoFiscal>> ListarComCertificadoVencendoAsync(
        DateTime limiteVencimento, CancellationToken cancellationToken = default);
}
