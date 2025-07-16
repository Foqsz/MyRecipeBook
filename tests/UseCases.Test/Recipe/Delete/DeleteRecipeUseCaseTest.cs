using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Delete;
public class DeleteRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () =>
        {
            await useCase.Execute(recipe.Id);
        };

        await act.ShouldNotThrowAsync(); //esperando que nao lance nenhuma excecao
    }


    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user); 

        var useCase = CreateUseCase(user, recipe);

        var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(recipeId: 1000));

        exception.ShouldNotBeNull();
        exception.GetErrorMessages().ShouldNotBeNull();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages()[0].ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    private static DeleteRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryRead = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var repositoryWrite = RecipeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new DeleteRecipeUseCase(loggedUser, repositoryRead, repositoryWrite, unitOfWork, blobStorage);
    }
}
