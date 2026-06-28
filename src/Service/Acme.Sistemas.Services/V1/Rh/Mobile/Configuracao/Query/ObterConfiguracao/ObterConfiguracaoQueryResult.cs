namespace Acme.Sistemas.Services.V1.Rh.Mobile.Configuracao.Query.ObterConfiguracao;

public sealed record VersaoMobileInfo(
    string MinimoSuportado,
    string Atual,
    bool ObrigatorioAtualizar,
    string? LinkAndroid,
    string? LinkIos);

public sealed record BannerMobileInfo(
    Guid Id, string Titulo, string Mensagem, string Tipo, DateTime? Expira);

public sealed record ObterConfiguracaoQueryResult(
    Guid TenantId,
    string TenantNome,
    string? LogoUrl,
    string CorPrimaria,
    VersaoMobileInfo Versao,
    IReadOnlyList<BannerMobileInfo> Banners);
