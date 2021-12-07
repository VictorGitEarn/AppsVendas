using Apps.APIRest.Configuration;
using Apps.APIRest.Configuration.JWT;
using Apps.Data.Config;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

var configuration = builder.Configuration;

services.AddApiConfiguration(configuration);

services.ConfigureMongoDb(configuration);

services.AddIdentityConfig(configuration);

services.Add_Services_Repository();

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
