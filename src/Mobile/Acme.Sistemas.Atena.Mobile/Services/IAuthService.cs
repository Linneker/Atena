using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;

namespace Acme.Sistemas.Atena.Mobile.Services;

public interface IAuthService
{
    Task<LoginMobileResponse?> LoginAsync(string email, string senha);
    Task<bool> RefreshAsync();
    Task LogoutAsync();
    Task<bool> EstaLogadoAsync();
}

public sealed class AuthService : IAuthService
{
    private readonly IAtenaApi _api;
    private readonly ISecureTokenStore _tokens;

    public AuthService(IAtenaApi api, ISecureTokenStore tokens)
    {
        _api = api;
        _tokens = tokens;
    }

    public async Task<LoginMobileResponse?> LoginAsync(string email, string senha)
    {
        var deviceId = await ObterOuCriarDeviceIdAsync();
        var plataforma = DeviceInfo.Current.Platform.ToString();

        try
        {
            var response = await _api.LoginMobileAsync(
                new LoginMobileRequest(email, senha, deviceId, plataforma));
            await _tokens.SaveTokensAsync(response.AccessToken, response.RefreshToken);
            return response;
        }
        catch (Refit.ApiException)
        {
            return null;
        }
    }

    public async Task<bool> RefreshAsync()
    {
        var refresh = await _tokens.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refresh)) return false;

        try
        {
            var response = await _api.RefreshAsync(new RefreshTokenRequest(refresh));
            // RefreshToken não muda no renovar — só access é renovado
            var currentRefresh = await _tokens.GetRefreshTokenAsync() ?? string.Empty;
            await _tokens.SaveTokensAsync(response.AccessToken, currentRefresh);
            return true;
        }
        catch (Refit.ApiException)
        {
            await _tokens.ClearAsync();
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try { await _api.LogoutAsync(); }
        catch { /* silencioso — refresh pode já estar revogado */ }
        await _tokens.ClearAsync();
    }

    public async Task<bool> EstaLogadoAsync()
        => !string.IsNullOrEmpty(await _tokens.GetAccessTokenAsync());

    private async Task<string> ObterOuCriarDeviceIdAsync()
    {
        const string key = "atena.deviceId";
        var existing = await _tokens.GetAsync(key);
        if (!string.IsNullOrEmpty(existing)) return existing;

        var newId = Guid.NewGuid().ToString("N");
        await _tokens.SaveAsync(key, newId);
        return newId;
    }
}
