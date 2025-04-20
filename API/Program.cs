using Infrastructure;
using Services;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptions();

builder.Services
    .AddOpenApi()
    .AddDatabase(builder.Configuration)
    .AddInfrastructure()
    .AddServices()
    .AddJwtAuth(builder.Configuration)
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); });
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();