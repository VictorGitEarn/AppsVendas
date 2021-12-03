using Apps.Data.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Data.Config
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(options => new MongoDbContext(configuration.GetConnectionString("AppsVendas")));

            return services;
        }
    }
}
