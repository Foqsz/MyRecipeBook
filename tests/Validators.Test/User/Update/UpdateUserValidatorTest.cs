using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de nome vazio
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY)
              .ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de email vazio
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.EMAIL_EMPTY)
              .ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invali()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de email invalido
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.EMAIL_INVALID)
              .ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
    }
}
