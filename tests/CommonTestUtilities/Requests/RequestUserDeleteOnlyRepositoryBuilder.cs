using Moq;
using MyRecipeBook.Application.UseCases.User.Delete.Request;

namespace CommonTestUtilities.Requests;
public class RequestUserDeleteOnlyRepositoryBuilder
{
    public static IRequestDeleteUserUseCase Build()
    {
        var mock = new Mock<IRequestDeleteUserUseCase>();

        return mock.Object;
    }
}
