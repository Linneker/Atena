using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using Refit;

namespace Acme.Sistemas.Atena.Mobile.Services.Api;

/// <summary>
/// Contrato Refit do backend Atena. Cobre Auth, Ponto, Espelho, Ajustes,
/// Dispositivos e Configuração mobile. Bearer token é injetado pelo
/// <see cref="AuthDelegatingHandler"/>; refresh em 401 é transparente.
/// </summary>
public interface IAtenaApi
{
    // ============================== Auth =====================================

    [Post("/api/v1/autenticacao/login-mobile")]
    Task<LoginMobileResponse> LoginMobileAsync([Body] LoginMobileRequest request);

    [Post("/api/v1/autenticacao/renovar-token")]
    Task<RefreshTokenResponse> RefreshAsync([Body] RefreshTokenRequest request);

    [Post("/api/v1/autenticacao/logout")]
    Task LogoutAsync();

    // ============================== Ponto ====================================

    /// <summary>Bate ponto com multipart (foto + form fields).</summary>
    [Multipart]
    [Post("/api/v1/rh/ponto/bater-mobile")]
    Task<BaterPontoResponse> BaterPontoMobileAsync(
        [AliasAs("foto")] StreamPart foto,
        [AliasAs("tipo")] string? tipo,
        [AliasAs("latitude")] decimal? latitude,
        [AliasAs("longitude")] decimal? longitude,
        [AliasAs("deviceId")] string deviceId,
        [AliasAs("timestampLocal")] DateTime timestampLocal,
        [AliasAs("hashBatida")] string hashBatida,
        [AliasAs("provaBiometriaLocal")] string? provaBiometriaLocal);

    [Get("/api/v1/rh/ponto/proprio")]
    Task<ListaMarcacoesResponse> ListarMarcacoesProprioAsync(
        [Query] DateOnly dataInicio, [Query] DateOnly dataFim);

    // ============================== Espelho ==================================

    [Get("/api/v1/rh/ponto/espelho")]
    Task<EspelhoWrapperDto> ObterEspelhoAsync(
        [Query] string funcionarioId, [Query] string competencia);

    // ============================== Ajustes ==================================

    [Post("/api/v1/rh/ponto/ajustes")]
    Task<CriadoResponse> SolicitarAjusteAsync([Body] SolicitarAjusteRequest request);

    [Get("/api/v1/rh/ponto/proprio/ajustes")]
    Task<ListaAjustesResponse> ListarMeusAjustesAsync();

    // ============================== Dispositivos =============================

    [Post("/api/v1/mobile/dispositivos/registrar")]
    Task<RegistrarDispositivoResponse> RegistrarDispositivoAsync(
        [Body] RegistrarDispositivoRequest request);

    [Post("/api/v1/mobile/dispositivos/{deviceId}/desregistrar")]
    Task DesregistrarDispositivoAsync(string deviceId);

    // ============================== Configuração =============================

    [Get("/api/v1/mobile/configuracao")]
    Task<ConfiguracaoMobileResponse> ObterConfiguracaoAsync();
}

public sealed record ListaMarcacoesResponse(IReadOnlyList<MarcacaoDto> Items, int Total);
public sealed record ListaAjustesResponse(IReadOnlyList<AjusteDto> Items, int Total);
public sealed record EspelhoWrapperDto(EspelhoMensalDto Espelho);
public sealed record CriadoResponse(string Id);
