using Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Extensions;

public static class ConfigureExtension
{
    public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));
        return builder;
    }
}