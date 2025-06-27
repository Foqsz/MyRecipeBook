using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastucture.DataAccess.Repositories;
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public RecipeRepository(MyRecipeBookDbContext context) => _dbContext = context;

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        //AsNoTracking por que não vai atualizar os dados, apenas ler
        var query = _dbContext
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        if (filters.Difficulties.Any()) // Verifica se a lista de dificuldades não está vazia
        {
            // Verifica se a dificuldade é nula e se a lista de dificuldades contém o valor
            query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
        }

        if (filters.CookingTimes.Any()) // verifica se existe uma lisa com tempo de preparo 
        {
            query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
        }

        if (filters.DishTypes.Any())
        {
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));
        }

        if(filters.RecipeTitle_Ingredient.NotEmpty())
        {
            query = query.Where(recipe =>
            recipe.Title.Contains(filters.RecipeTitle_Ingredient)
            || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));
        }

        return await query.ToListAsync();
    }
}
