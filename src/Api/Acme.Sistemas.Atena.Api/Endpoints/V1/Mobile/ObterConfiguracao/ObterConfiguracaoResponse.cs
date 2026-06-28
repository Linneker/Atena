namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.ObterConfiguracao;

public sealed record VersaoMobileResponse(
    string MinimoSuportado,
    string Atual,
    bool ObrigatorioAtualizar,
    string? LinkAndroid,
    string? LinkIos);

public sealed record BannerMobileResponse(
    Guid Id, string Titulo, string Mensagem, string Tipo, DateTime? Expira);

public sealed record ObterConfiguracaoResponse(
    Guid TenantId,
    string TenantNome,
    string? LogoUrl,
    string CorPrimaria,
    VersaoMobileResponse Versao,
    IReadOnlyList<BannerMobileResponse> Banners);
