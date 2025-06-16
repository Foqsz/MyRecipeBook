namespace MyRecipeBook.Domain.Security.Tokens;
public interface IAccessTokenValidator
{
    public Guid ValidateAnGetUserIdentifier(string token);
}
