using Moq;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace CommonTestUtilities.ServiceBus;
public class DeleteUserQueueBuilder
{
    public static IDeleteUserQueue Build()
    {
        var mock = new Mock<IDeleteUserQueue>();

        return mock.Object;
    } 
}
