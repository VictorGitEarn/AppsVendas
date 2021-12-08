using Apps.Data.Base;
using Apps.Data.Repository;
using Apps.Helpers;
using Apps.Payment.Client;
using Apps.Payment.Client.Consumer;
using Apps.Services.Implementation;
using Apps.Services.Interfaces;
using Apps.Services.Observer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((h, services) =>
        services.AddHostedService<Worker>()
        .AddSingleton(options => new MongoDbContext(h.Configuration.GetConnectionString("AppsVendas")))
        .AddSingleton<IEncrypt_Decrypt>(x => new Encrypt_Decrypt(h.Configuration.GetSection("key").Value))
        .AddScoped<PurchaseRepository>()
        .AddScoped<CreditCardRepository>()
        .AddScoped<PaymentConsumer>()
        .AddScoped<PaymentRepository>()
        .AddScoped<IPaymentObserverService, PaymentObserverService>()
        .AddScoped<IPaymentService, PaymentService>()
        )
    .Build();

await host.StartAsync();