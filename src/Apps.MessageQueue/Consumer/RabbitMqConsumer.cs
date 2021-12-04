using Apps.MessageQueue.Configuration;
using MassTransit;

namespace Apps.MessageQueue.Consumer
{
    public class RabbitMqConsumer : IDisposable
    {
        private readonly IBusControl _bus;

        public RabbitMqConsumer(RabbitMQConfig configuration, IList<IConsumerAttach> consumer)
        {
            _bus = ConfigureBus(configuration, consumer);
        }

        public void Start() => _bus.Start();
        public void Stop() => _bus.Stop();
        public void Dispose() => Stop();

        private static IBusControl ConfigureBus(RabbitMQConfig configuration, IList<IConsumerAttach> consumers)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(configuration.Uri), h =>
                {
                    h.Username(configuration.Username);
                    h.Password(configuration.Password);
                });

                foreach (var consumer in consumers)
                {
                    consumer.Attach(cfg);
                }
            });
        }
    }
}
