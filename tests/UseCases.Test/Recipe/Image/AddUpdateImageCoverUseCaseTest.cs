using Azure.Core;
using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image;
public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.ShouldNotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Error_Recipe_NotFound(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(1, file));
         
        act.GetErrorMessages().ShouldNotBeNull();
        act.GetErrorMessages().Count.ShouldBe(1);
        act.GetErrorMessages()[0].ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var file = FormFileBuilder.Txt();

        var act = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(recipe.Id, file));

        act.GetErrorMessages().ShouldNotBeNull();
        act.GetErrorMessages().Count.ShouldBe(1);
        act.GetErrorMessages()[0].ShouldBe(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
    }

    private static AddUpdateImageCoverUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    { 
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new AddUpdateImageCoverUseCase(loggedUser, repository, unitOfWork, blobStorage);
    }
}
