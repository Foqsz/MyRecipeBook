﻿using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Dashboard;
using Shouldly;

namespace UseCases.Test.Dashboard;
public class GetDashboardUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.Count.ShouldBeGreaterThan(0);
        result.Recipes.Select(r => r.Id).ShouldBeUnique();

        foreach (var recipe in result.Recipes)
        {
            recipe.Id.ShouldNotBeNullOrWhiteSpace();
            recipe.Title.ShouldNotBeNullOrWhiteSpace();
            recipe.AmountIngredients.ShouldBeGreaterThan(0);
            recipe.ImageUrl.ShouldNotBeNullOrWhiteSpace();
        } 
    }

    private static GetDashboardUseCase CreateUseCase(
        MyRecipeBook.Domain.Entities.User user, 
        IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {   
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();

        return new GetDashboardUseCase(repository, mapper, loggedUser, blobStorage);
    }
}
