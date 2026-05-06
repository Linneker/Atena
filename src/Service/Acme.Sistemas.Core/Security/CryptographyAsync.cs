using System.Security.Cryptography;
using System.Text;

namespace Acme.Sistemas.Core.Security;

public sealed class CryptographyAsync
{
    private readonly RSA _rsa;

    public CryptographyAsync(RSA rsa)
    {
        _rsa = rsa;
    }

    public Task<byte[]> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cipher = _rsa.Encrypt(plaintext, RSAEncryptionPadding.OaepSHA512);
        return Task.FromResult(cipher);
    }

    public Task<byte[]> DecryptAsync(byte[] ciphertext, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var plain = _rsa.Decrypt(ciphertext, RSAEncryptionPadding.OaepSHA512);
        return Task.FromResult(plain);
    }

    public async Task<string> EncryptStringAsync(string plaintext, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var cipher = await EncryptAsync(bytes, cancellationToken);
        return Convert.ToBase64String(cipher);
    }

    public async Task<string> DecryptStringAsync(string base64Ciphertext, CancellationToken cancellationToken = default)
    {
        var bytes = Convert.FromBase64String(base64Ciphertext);
        var plain = await DecryptAsync(bytes, cancellationToken);
        return Encoding.UTF8.GetString(plain);
    }
}
