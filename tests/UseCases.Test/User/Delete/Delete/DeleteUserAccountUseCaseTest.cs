using Azure.Core;
using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Delete.Delete;
using Shouldly;

namespace UseCases.Test.User.Delete.Delete;
public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(user.UserIdentifier);

        await act.ShouldNotThrowAsync();
    } 

    private static DeleteUserAccountUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    { 
        var repositoryDelete = UserDeleteAccountOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new DeleteUserAccountUseCase(repositoryDelete, unitOfWork, blobStorage);
    }
}
