﻿using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Dashboard;
public class GetDashboardInvalidTokenTest : MyRecipeBookClassFixture
{
    private const string METHOD = "dashboard";

    public GetDashboardInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(METHOD, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(METHOD, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuild.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
