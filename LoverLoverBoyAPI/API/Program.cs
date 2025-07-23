using API;
using Infrastructure;
using Services;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptions();

builder.Services
    .AddDatabase(builder.Configuration)
    .AddInfrastructure()
    .AddServices()
    .AddJwtAuth(builder.Configuration)
    .AddApi();

var app = builder.Build();
await app.UseInfrastructureAsync();
app.UseApi();
app.Run();