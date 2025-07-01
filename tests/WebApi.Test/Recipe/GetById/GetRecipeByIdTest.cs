using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.GetById;
public class GetRecipeByIdTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";

    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    private readonly string _recipeTitle;

    public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
        _recipeTitle = factory.GetRecipeTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuild.Build().Generate(_userIdentifier);

        var response = await DoGet($"{METHOD}/{_recipeId}", token: token); 
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldBe(_recipeId);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(_recipeTitle);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_NotFound(string culture)
    {
        var token = JwtTokenGeneratorBuild.Build().Generate(_userIdentifier);

        var id = IdEncripterBuilder.Build().Encode(1000);

        var response = await DoGet($"{METHOD}/{id}", token: token, culture: culture);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new System.Globalization.CultureInfo(culture));

        errors.Count().ShouldBe(1);
        errors.Any(c => c.GetString() == expectedMessage).ShouldBeTrue();
    }
}
