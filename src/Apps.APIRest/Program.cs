using Apps.APIRest.Configuration;
using Apps.APIRest.Configuration.JWT;
using Apps.APIRest.Models;
using Apps.Data.Config;
using Apps.Domain.Business.Interfaces;
using Apps.Domain.Business.Notes;
using Apps.Services.Implementation;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

var configuration = builder.Configuration;

services.AddApiConfiguration();

services.ConfigureMongoDb(configuration);

services.AddIdentityConfig(configuration);

#region Servicos
services.AddScoped<INotes, Note>();
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IUser, MongoAppUser>();
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "Redis_";
});
services.AddScoped<IProductService, ProductService>();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerConfig(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());

app.Run();
