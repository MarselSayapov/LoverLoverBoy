using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Middlewares;
using Services.Services;

namespace Services;

public static class ServiceStartup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}