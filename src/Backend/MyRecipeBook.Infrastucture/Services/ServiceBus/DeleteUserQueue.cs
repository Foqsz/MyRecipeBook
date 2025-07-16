using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Infrastucture.Services.ServiceBus;
public class DeleteUserQueue : IDeleteUserQueue
{
    public Task SendMessage(User user)
    {
        throw new NotImplementedException();
    }
}
