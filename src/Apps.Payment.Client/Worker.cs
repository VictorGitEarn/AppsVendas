using Apps.MessageQueue;
using Apps.Payment.Client.Consumer;
using Apps.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Payment.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly List<IConsumerApps> _consumers;

        public Worker(ILogger<Worker> logger, PaymentConsumer paymentConsumer)
        {
            _logger = logger;
            _consumers = new List<IConsumerApps>()
            {
                paymentConsumer
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    _consumers.ForEach(consumer =>
                    {
                        consumer.Attach(cfg);
                    });
                });
                
                try
                {
                    busControl.Start();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Worker error: {ex}", ex);
                }
                finally
                {
                    busControl.Stop();
                }
            }
        }
    }
}
