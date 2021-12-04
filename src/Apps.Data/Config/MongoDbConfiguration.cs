using Apps.Data.Base;
using Apps.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Data.Config
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(options => new MongoDbContext(configuration.GetConnectionString("AppsVendas")));

            services.AddScoped<AppsToSellRepository>();

            return services;
        }
    }
}
