using System.Security.Cryptography;
using System.Text;

namespace Elderforge.Core.Server.Utils;

public static class HashUtils
{
    public static string CreateBCryptHash(this string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input);
    }

    public static string Sha1Hash(this string input)
    {
        return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(input)));
    }
}
