namespace API;

public static class ApiStartup
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static void UseApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app
            .UseExceptionHandler()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
            });

        app.MapControllers();
    }
}