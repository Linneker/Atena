namespace Acme.Sistemas.Atena.Mobile.Services;

public interface ISecureTokenStore
{
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();
    Task SaveTokensAsync(string accessToken, string refreshToken);
    Task ClearAsync();
    Task<string?> GetAsync(string key);
    Task SaveAsync(string key, string value);
}

/// <summary>
/// Wrapper de Microsoft.Maui.Storage.SecureStorage que centraliza chaves
/// (accessToken, refreshToken, deviceId, ...). Em Android usa Keystore +
/// EncryptedSharedPreferences; em iOS/macOS usa Keychain; em Windows PasswordVault.
/// </summary>
public sealed class SecureTokenStore : ISecureTokenStore
{
    private const string KeyAccess = "atena.accessToken";
    private const string KeyRefresh = "atena.refreshToken";

    public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(KeyAccess);
    public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(KeyRefresh);

    public async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        await SecureStorage.Default.SetAsync(KeyAccess, accessToken);
        await SecureStorage.Default.SetAsync(KeyRefresh, refreshToken);
    }

    public Task ClearAsync()
    {
        SecureStorage.Default.RemoveAll();
        return Task.CompletedTask;
    }

    public Task<string?> GetAsync(string key) => SecureStorage.Default.GetAsync(key);
    public Task SaveAsync(string key, string value) => SecureStorage.Default.SetAsync(key, value);
}
