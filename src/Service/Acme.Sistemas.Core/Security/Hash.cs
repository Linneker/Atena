using System.Security.Cryptography;
using System.Text;

namespace Acme.Sistemas.Core.Security;

public static class Hash
{
    public static string Sha256(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    public static string Sha512(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA512.HashData(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    public static bool FixedTimeEquals(string left, string right)
    {
        if (left.Length != right.Length) return false;
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
