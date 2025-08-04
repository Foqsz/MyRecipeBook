﻿using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter;
public class FilterRecipeTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe/filter";

    private readonly Guid _userIdentifier;

    private string _recipeTitle;
    private MyRecipeBook.Domain.Enums.Difficulty _recipeDifficultyLevel;
    private MyRecipeBook.Domain.Enums.CookingTime _recipeCookingTime;
    private IList<MyRecipeBook.Domain.Enums.DishType> _recipeDishTypes;

    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();

        _recipeTitle = factory.GetRecipeTitle();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipeDifficultyLevel = factory.GetRecipeDifficulty();
        _recipeDishTypes = factory.GetDishTypes();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterRecipeJson
        {
            CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_recipeCookingTime],
            Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_recipeDifficultyLevel],
            DishTypes = _recipeDishTypes.Select(dishType => (MyRecipeBook.Communication.Enums.DishType)dishType).ToList(),
            RecipeTitle_Ingredient = _recipeTitle,
        };

        var token = JwtTokenGeneratorBuild.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipes = responseData.RootElement.GetProperty("recipes").EnumerateArray().ToList();
        recipes.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDonExist";

        var token = JwtTokenGeneratorBuild.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        var token = JwtTokenGeneratorBuild.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new System.Globalization.CultureInfo(culture));

        errors.Count().ShouldBe(1);
        errors.Any(c => c.GetString() == expectedMessage).ShouldBeTrue();
    }
}
