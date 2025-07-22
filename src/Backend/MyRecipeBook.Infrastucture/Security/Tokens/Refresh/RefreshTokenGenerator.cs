using MyRecipeBook.Domain.Security.Tokens.RefreshToken;

namespace MyRecipeBook.Infrastucture.Security.Tokens.Refresh;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
