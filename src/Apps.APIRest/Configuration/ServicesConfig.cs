using Apps.APIRest.Models;
using Apps.Data.Repository;
using Apps.Domain.Business.Interfaces;
using Apps.Domain.Business.Notes;
using Apps.Services.Implementation;
using Apps.Services.Interfaces;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Apps.APIRest.Configuration
{
    public static class ServicesConfig
    {
        public static IServiceCollection Add_Services_Repository(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<INotes, Note>();
            services.AddScoped<IUser, MongoAppUser>();

            services.AddScoped<ProductRepository>();
            services.AddScoped<IProductService, ProductService>();


            services.AddScoped<IPaymentService, PaymentService>();


            services.AddScoped<IPurchaseService, PurchaseService>();

            return services;
        }
    }
}
