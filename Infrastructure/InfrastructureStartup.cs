using Infrastructure.Contexts;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
        return builder;
    }
}