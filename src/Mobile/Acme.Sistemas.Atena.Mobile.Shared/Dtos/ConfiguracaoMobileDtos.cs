namespace Acme.Sistemas.Atena.Mobile.Shared.Dtos;

public sealed record ConfiguracaoMobileResponse(
    string TenantId,
    string TenantNome,
    string? LogoUrl,
    string CorPrimaria,
    VersaoMobileDto Versao,
    JornadaMobileDto? JornadaVigente,
    IReadOnlyList<BannerMobileDto> Banners);

public sealed record VersaoMobileDto(
    string MinimoSuportado,
    string Atual,
    bool ObrigatorioAtualizar,
    string? LinkAndroid,
    string? LinkIos);

public sealed record JornadaMobileDto(
    string Nome,
    decimal CargaSemanal,
    int ToleranciaMinutos,
    string JanelasJson);

public sealed record BannerMobileDto(
    string Id,
    string Titulo,
    string Mensagem,
    string Tipo,            // "info" | "warning" | "success"
    DateTime? Expira);
