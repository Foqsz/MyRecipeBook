using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using OpenAI.Chat;

namespace MyRecipeBook.Domain.Services.OpenAI;
public class ChatGPTService : IGenerateRecipeAI
{ 
    private readonly ChatClient _chatClient;

    public ChatGPTService(ChatClient chatClient) => _chatClient = chatClient;

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE),
            new UserChatMessage(string.Join(";", ingredients))
        };

        var completion = await _chatClient.CompleteChatAsync(messages);

        //não quero itens onde eles são espaços em branco
        var responseList = completion.Value.Content[0].Text
            .Split("\n") //aqui ele gera posições []
            .Where(response => response.Trim().Equals(string.Empty).IsFalse()) 
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"), //vai devolver um array de strings onde cada item vai ser um ingrediente
            Instructions = responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()
        };
    }
}
