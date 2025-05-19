using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptografhy;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;   

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        var criptografiaDeSenha = new PasswordEncripter();

        var autoMapper = new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMappingProfile());
        }).CreateMapper();

        Validate(request);
          
        var user = autoMapper.Map<Domain.Entities.User>(request);
 
        user.Password = criptografiaDeSenha.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(user);

        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }

    private void Validate(RequestRegisterUserJson request)  
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
