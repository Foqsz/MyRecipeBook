using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;
public interface IGenerateRecipeUseCase
{
    public Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
}
