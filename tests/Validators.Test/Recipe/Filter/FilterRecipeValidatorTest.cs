using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe.Filter;
public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator =  new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);

        var result = validator.Validate(request);   

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    }   

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator =  new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties.Add((Difficulty)1000);

        var result = validator.Validate(request);   

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
    }   

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator =  new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);   

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
    }   
}
