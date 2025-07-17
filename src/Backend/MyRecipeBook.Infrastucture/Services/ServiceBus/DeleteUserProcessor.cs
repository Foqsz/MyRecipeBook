using Azure.Messaging.ServiceBus;

namespace MyRecipeBook.Infrastucture.Services.ServiceBus;
public class DeleteUserProcessor
{
    private readonly ServiceBusProcessor _processor;

    public DeleteUserProcessor(ServiceBusProcessor processor)
    {
        _processor = processor;
    }

    public ServiceBusProcessor GetProcessor() => _processor;
}
