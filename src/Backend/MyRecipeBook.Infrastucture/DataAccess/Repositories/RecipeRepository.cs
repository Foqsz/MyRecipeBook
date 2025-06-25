using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastucture.DataAccess.Repositories;
public class RecipeRepository  : IRecipeWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;
    
    public RecipeRepository(MyRecipeBookDbContext context) => _dbContext = context;

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
}
