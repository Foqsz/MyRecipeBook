using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Infrastucture.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Sha512Encripter("abc123456");
}
