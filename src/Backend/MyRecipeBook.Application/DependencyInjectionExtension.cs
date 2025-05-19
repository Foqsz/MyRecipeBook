using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptografhy;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;
public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddPasswordsEncrpter(services);
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {  
        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMappingProfile());
        }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddPasswordsEncrpter(IServiceCollection services)
    {
        services.AddScoped(option => new PasswordEncripter());
    }
}
