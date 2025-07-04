using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Dashboard;
public interface IGetDashboardUseCase
{
    public Task<ResponseRecipesJson> Execute();
}
