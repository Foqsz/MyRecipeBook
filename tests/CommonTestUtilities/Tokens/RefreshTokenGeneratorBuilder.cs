using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens.RefreshToken;
using MyRecipeBook.Infrastucture.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;
public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}
