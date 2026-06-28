using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

public sealed class ObterMeuBrandingQueryHandler
    : IRequestHandler<ObterMeuBrandingQuery, ResponseDefault<ObterMeuBrandingQueryResult>>
{
    // Defaults usados quando o tenant não definiu cores.
    private const string DefaultPrimaria   = "#321fdb";
    private const string DefaultSecundaria = "#3c4b64";
    private const string DefaultAccent     = "#2eb85c";

    private readonly ITenantRepository _tenants;
    private readonly ITenantContext _tenantContext;

    public ObterMeuBrandingQueryHandler(ITenantRepository tenants, ITenantContext tenantContext)
    {
        _tenants = tenants;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ObterMeuBrandingQueryResult>> Handle(
        ObterMeuBrandingQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await _tenants.GetByIdAsync(_tenantContext.TenantId, cancellationToken);
        if (tenant is null)
        {
            return ResponseDefault<ObterMeuBrandingQueryResult>.NotFound("Tenant não encontrado.");
        }

        var result = new ObterMeuBrandingQueryResult(
            tenant.Id,
            tenant.RazaoSocial,
            tenant.LogoUrl,
            string.IsNullOrWhiteSpace(tenant.CorPrimaria) ? DefaultPrimaria : tenant.CorPrimaria!,
            DefaultSecundaria,
            DefaultAccent);

        return ResponseDefault<ObterMeuBrandingQueryResult>.Ok(result);
    }
}
