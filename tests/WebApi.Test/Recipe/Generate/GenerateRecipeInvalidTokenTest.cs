using Azure.Core;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Request;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.Generate;
public class GenerateRecipeInvalidTokenTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe/generate";
    
    public GenerateRecipeInvalidTokenTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    { 
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Invalid()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build(); 

        var token = JwtTokenGeneratorBuild.Build().Generate(Guid.NewGuid());

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
