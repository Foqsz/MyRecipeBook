using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastucture.DataAccess;
using MyRecipeBook.Infrastucture.DataAccess.Repositories;

namespace MyRecipeBook.Infrastucture;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddRepositories(services);
        AddDbContext_SqlServer(services);
    }

    private static void AddDbContext_SqlServer(IServiceCollection services)
    {
        var connectionString = "Data Source=PC-VP;Initial Catalog=meulivrodereceitas;User ID=sa;Password=1967;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }
}
