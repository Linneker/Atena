using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Configuracao.Query.ObterConfiguracao;

/// <summary>
/// Configuração entregue ao app no boot. Versão mínima/atual ficam hardcoded por release
/// — em W4 vira tabela configurável.
/// </summary>
public sealed class ObterConfiguracaoQueryHandler
    : IRequestHandler<ObterConfiguracaoQuery, ResponseDefault<ObterConfiguracaoQueryResult>>
{
    private const string VersaoMinima = "1.0.0";
    private const string VersaoAtual = "1.0.0";

    private readonly ITenantRepository _tenants;
    private readonly ITenantContext _tenantContext;

    public ObterConfiguracaoQueryHandler(ITenantRepository tenants, ITenantContext tenantContext)
    {
        _tenants = tenants;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ObterConfiguracaoQueryResult>> Handle(
        ObterConfiguracaoQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenants.GetByIdAsync(_tenantContext.TenantId, cancellationToken);
        if (tenant is null)
            return ResponseDefault<ObterConfiguracaoQueryResult>.NotFound("Tenant não encontrado.");

        var result = new ObterConfiguracaoQueryResult(
            TenantId: tenant.Id,
            TenantNome: tenant.RazaoSocial,
            LogoUrl: null,             // W4 lerá de branding
            CorPrimaria: "#1d4ed8",
            Versao: new VersaoMobileInfo(
                MinimoSuportado: VersaoMinima,
                Atual: VersaoAtual,
                ObrigatorioAtualizar: false,
                LinkAndroid: "https://play.google.com/store/apps/details?id=br.com.acme.atena.mobile",
                LinkIos: "https://apps.apple.com/app/atena-mobile/id000000000"),
            Banners: Array.Empty<BannerMobileInfo>());

        return ResponseDefault<ObterConfiguracaoQueryResult>.Ok(result);
    }
}
