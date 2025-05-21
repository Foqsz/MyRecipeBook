using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptografhy;

public class PasswordEncripter
{
    private readonly string _additionalKey;

    public PasswordEncripter(string additionalKey) => _additionalKey = additionalKey;

    public string Encrypt(string password)
    {
        var newPassword = $"{password}{_additionalKey}";

        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        return StringBytes(hashBytes);
    }

    /// <summary>
    /// convertendo um array de bytes de volta em string
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
