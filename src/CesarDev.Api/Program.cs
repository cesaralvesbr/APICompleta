using CesarDev.Api.Configuration;
using CesarDev.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

IdentityConfiguration.AddIdentityConfiguration(builder.Services, builder.Configuration, connectionString);
builder.Services.AddDbContext<DevDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddLoggingConfig(builder.Configuration);
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), name:"BancoSQLL");
builder.Services.AddHealthChecksUI();

DependencyInjectionConfig.ResolveDependencies(builder.Services);

var app = builder.Build();

//Configuração
app.UseApiConfig(app.Environment);

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(apiVersionDescriptionProvider);
app.UseLoggingConfiguration();
app.UseHealthChecks("/api/hc",new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/api/hc-ui";
});
app.Run();
