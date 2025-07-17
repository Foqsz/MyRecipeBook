using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.ServiceBus;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
using Shouldly;

namespace UseCases.Test.User.Delete.Request;
public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.ShouldNotThrowAsync();

        user.Active.ShouldBeFalse(); 
    }

    private static RequestDeleteUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var queue = DeleteUserQueueBuilder.Build();
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build(); 

        return new RequestDeleteUserUseCase(queue, repository, loggedUser, unitOfWork);
    }
}
