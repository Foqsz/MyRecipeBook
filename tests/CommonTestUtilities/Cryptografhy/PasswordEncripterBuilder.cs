using MyRecipeBook.Application.Services.Cryptografhy;

namespace CommonTestUtilities.Cryptografhy;
public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new PasswordEncripter("abc123456");
}
