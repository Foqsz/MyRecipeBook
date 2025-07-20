using Moq;
using MyRecipeBook.Application.UseCases.Login.External;

namespace CommonTestUtilities.Google;
public class UseExternalLoginUseCaseBuilderTest
{
    public static IExternalLoginUseCase Build()
    {
        var mock = new Mock<IExternalLoginUseCase>();

        return mock.Object;
    }
}
