using Apps.Helpers;
using Apps.Payment.Client;
using Apps.Payment.Client.Consumer;
using Apps.Services.Implementation;
using Apps.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((h, services) =>
        services.AddHostedService<Worker>()
        .AddScoped<IPaymentService, PaymentService>()
        .AddScoped<PaymentConsumer>()
        .AddSingleton<IEncrypt_Decrypt>(x => new Encrypt_Decrypt("899622d998be2d4d"))
        )
    .Build();

await host.StartAsync();