using CommonTestUtilities.Entities;
using CommonTestUtilities.Google;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.UseCases.Login.External;
using Shouldly;

namespace UseCases.Test.Login.Google;
public class ExternalLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var token = JwtTokenGeneratorBuild.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(user.Name, user.Email);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_User_Dont_Exist()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(name: user.Name, email: user.Email);

        result.ShouldNotBeNullOrEmpty();
    }

    private static ExternalLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    { 
        var userReadOnly = new UserReadOnlyRepositoryBuilder().Build();
        var userWriteOnly = UserWriteOnlyRepositoryBuilder.Build(); ;
        var unitOfWork = UnitOfWorkBuilder.Build();
        var token = JwtTokenGeneratorBuild.Build(); 

        if(user is not null)
            userReadOnly.GetByEmail(user.Email);

        return new ExternalLoginUseCase(userReadOnly, userWriteOnly, unitOfWork, token);
    }
}
