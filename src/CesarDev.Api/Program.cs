using CesarDev.Api.Configuration;
using CesarDev.Business.Interfaces;
using CesarDev.Data.Context;
using CesarDev.Data.Repository;
using Microsoft.AspNetCore.Mvc;
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

DependencyInjectionConfig.ResolveDependencies(builder.Services);

var app = builder.Build();

//Configuração
app.UseApiConfig(app.Environment);

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(apiVersionDescriptionProvider);
app.UseLoggingConfiguration();
app.Run();
