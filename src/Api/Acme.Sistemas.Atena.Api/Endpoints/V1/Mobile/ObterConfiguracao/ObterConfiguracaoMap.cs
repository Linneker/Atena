using Acme.Sistemas.Services.V1.Rh.Mobile.Configuracao.Query.ObterConfiguracao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.ObterConfiguracao;

public static class ObterConfiguracaoMap
{
    public static ObterConfiguracaoQuery ToQuery(this ObterConfiguracaoRequest _) => new();

    public static ObterConfiguracaoResponse ToResponse(this ObterConfiguracaoQueryResult r)
        => new(r.TenantId, r.TenantNome, r.LogoUrl, r.CorPrimaria,
            new VersaoMobileResponse(r.Versao.MinimoSuportado, r.Versao.Atual,
                r.Versao.ObrigatorioAtualizar, r.Versao.LinkAndroid, r.Versao.LinkIos),
            r.Banners.Select(b => new BannerMobileResponse(
                b.Id, b.Titulo, b.Mensagem, b.Tipo, b.Expira)).ToList());
}
