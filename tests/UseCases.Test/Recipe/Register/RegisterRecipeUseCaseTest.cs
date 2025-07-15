using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe;
public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success_Without_Image()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success_With_Image(IFormFile file)
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build(file);

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => act());

        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
    }

    [Fact]
    public async Task Error_Invalid_File()
    {
        (var user, _) = UserBuilder.Build();

        var textFile = FormFileBuilder.Txt();

        var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

        var useCase = CreateUseCase(user);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));

        exception.ShouldNotBeNull();
        exception.GetErrorMessages().ShouldNotBeNull();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages()[0].ShouldBe(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
    }

    private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unitOfWork, mapper, blobStorage);
    }
}
