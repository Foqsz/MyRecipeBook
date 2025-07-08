using CommonTestUtilities.Dtos;
using CommonTestUtilities.OpenAI;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Generate;
public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var dto = GenerateRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(dto);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(dto.Title);
        result.CookingTime.ShouldBe((MyRecipeBook.Communication.Enums.CookingTime)dto.CookingTime);
        result.Difficulty.ShouldBe(MyRecipeBook.Communication.Enums.Difficulty.Low);
    }

    [Fact]
    public async Task Error_Duplicated_Ingredients()
    {
        var dto = GenerateRecipeDtoBuilder.Build();

        var request  = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients[0]);

        var useCase = CreateUseCase(dto);

        var act = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));

        act.ShouldNotBeNull();
        act.GetErrorMessages().ShouldNotBeNull();
        act.GetErrorMessages().Count.ShouldBe(1);
        act.GetErrorMessages()[0].ShouldBe(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
    }

    private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
    {
        var generateRecipeAI = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateRecipeAI);
    }
}
