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
        private readonly PaymentConsumer _consumer;

        public Worker(ILogger<Worker> logger, PaymentConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
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

                    _consumer.Attach(cfg);
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
