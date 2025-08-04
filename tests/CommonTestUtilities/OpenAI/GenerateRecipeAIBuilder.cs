using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.OpenAI;

namespace CommonTestUtilities.OpenAI;
public class GenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
    {
        var mock = new Mock<IGenerateRecipeAI>();


        //quando alguem chamar com qual quer string, retorna o dto
        mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

        return mock.Object;
    }
}
