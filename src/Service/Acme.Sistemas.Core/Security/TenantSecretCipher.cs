using System.Security.Cryptography;
using System.Text;

namespace Acme.Sistemas.Core.Security;

/// <summary>
/// AES-GCM com chave derivada por tenant a partir de uma master key.
/// Cipher and nonce devem ser persistidos juntos (mas separados);
/// a master key vem de configuração e nunca trafega pelo banco.
/// </summary>
public sealed class TenantSecretCipher
{
    private readonly byte[] _masterKey;

    public TenantSecretCipher(string masterKeyBase64OrText)
    {
        _masterKey = TryFromBase64(masterKeyBase64OrText)
            ?? SHA256.HashData(Encoding.UTF8.GetBytes(masterKeyBase64OrText));
    }

    public (byte[] CipherText, string NonceBase64) Encrypt(byte[] plaintext, Guid tenantId)
    {
        var key = DeriveKey(tenantId);
        var nonce = RandomNumberGenerator.GetBytes(12);
        var cipher = new byte[plaintext.Length];
        var tag = new byte[16];
        using var aes = new AesGcm(key, tagSizeInBytes: 16);
        aes.Encrypt(nonce, plaintext, cipher, tag);

        var combined = new byte[cipher.Length + tag.Length];
        Buffer.BlockCopy(cipher, 0, combined, 0, cipher.Length);
        Buffer.BlockCopy(tag, 0, combined, cipher.Length, tag.Length);
        return (combined, Convert.ToBase64String(nonce));
    }

    public byte[] Decrypt(byte[] ciphertextWithTag, string nonceBase64, Guid tenantId)
    {
        var key = DeriveKey(tenantId);
        var nonce = Convert.FromBase64String(nonceBase64);
        var cipher = new byte[ciphertextWithTag.Length - 16];
        var tag = new byte[16];
        Buffer.BlockCopy(ciphertextWithTag, 0, cipher, 0, cipher.Length);
        Buffer.BlockCopy(ciphertextWithTag, cipher.Length, tag, 0, 16);
        var plain = new byte[cipher.Length];
        using var aes = new AesGcm(key, tagSizeInBytes: 16);
        aes.Decrypt(nonce, cipher, tag, plain);
        return plain;
    }

    private byte[] DeriveKey(Guid tenantId)
    {
        var info = Encoding.UTF8.GetBytes($"acme-tenant-{tenantId}");
        return HKDF.DeriveKey(HashAlgorithmName.SHA256, _masterKey, outputLength: 32, salt: info, info: info);
    }

    private static byte[]? TryFromBase64(string s)
    {
        try { var b = Convert.FromBase64String(s); return b.Length >= 16 ? b : null; }
        catch { return null; }
    }
}
