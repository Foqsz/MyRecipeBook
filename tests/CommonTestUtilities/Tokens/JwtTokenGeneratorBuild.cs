using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastucture.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;
public class JwtTokenGeneratorBuild
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "ttttttttttttttttttttttttttttttttttttttttt");
}
