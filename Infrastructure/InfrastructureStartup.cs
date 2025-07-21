using Domain.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.UnitOfWork;
using Microsoft.AspNetCore.Builder;
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
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
        
        return builder;
    }
    
    public static async Task UseInfrastructureAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        await db.Database.EnsureCreatedAsync();
    }
}