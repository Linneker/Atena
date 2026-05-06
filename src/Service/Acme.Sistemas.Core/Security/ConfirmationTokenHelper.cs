using System.Security.Cryptography;

namespace Acme.Sistemas.Core.Security;

public static class ConfirmationTokenHelper
{
    public static string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(48);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    public static string HashToken(string token) => Hash.Sha256(token);
}
