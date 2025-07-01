﻿using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.GetById;
public class GetRecipeByIdInvalidTokenTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";

    public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{id}", token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Invalid()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{id}", token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuild.Build().Generate(Guid.NewGuid());

        var response = await DoGet($"{METHOD}/{id}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
