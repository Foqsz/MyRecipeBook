using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Sucess()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        /*NATIVO ASSERT .NET*/
        //Assert.True(result.IsValid);

        /* FLUENT ASSERTIONS*/
        //result.IsValid.Should().BeTrue();

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
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
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de email vazio
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.EMAIL_EMPTY)
              .ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "oi.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de email invalido
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.EMAIL_INVALID)
              .ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLenght)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLenght); 

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        // Deve retornar um único erro com a mensagem de senha invalida
        result.Errors.Single(e => e.ErrorMessage == ResourceMessagesException.PASSWORD_INVALID)
              .ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_INVALID);
    }
}
