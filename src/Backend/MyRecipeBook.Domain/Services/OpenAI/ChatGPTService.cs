using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.OpenAI;
public class ChatGPTService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-4o";

    public Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        throw new NotImplementedException();
    }
}
