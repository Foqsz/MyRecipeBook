using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Token.RefreshToken;
public interface ITokenRepository
{
    Task<Domain.Entities.RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken (Domain.Entities.RefreshToken refreshToken);
}
