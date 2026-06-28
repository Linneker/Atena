using System.Net;
using System.Net.Http.Headers;

namespace Acme.Sistemas.Atena.Mobile.Services;

/// <summary>
/// Injeta Bearer token em toda request. Em 401 tenta refresh; se refresh falhar,
/// limpa tokens e força redirect pra Login (via INotificationService).
/// </summary>
public sealed class AuthDelegatingHandler : DelegatingHandler
{
    private readonly ISecureTokenStore _tokens;
    private readonly Lazy<IAuthService> _auth;

    public AuthDelegatingHandler(ISecureTokenStore tokens, IServiceProvider sp)
    {
        _tokens = tokens;
        // Lazy para quebrar ciclo: AuthService depende de IAtenaApi que depende deste handler.
        _auth = new Lazy<IAuthService>(() => sp.GetRequiredService<IAuthService>());
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokens.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        // Tenta refresh
        var refreshed = await _auth.Value.RefreshAsync();
        if (!refreshed) return response;

        // Retry com token novo
        var newToken = await _tokens.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        return await base.SendAsync(request, cancellationToken);
    }
}
