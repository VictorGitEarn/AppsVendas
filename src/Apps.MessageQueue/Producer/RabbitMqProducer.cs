using Apps.MessageQueue.Configuration;
using MassTransit;

namespace Apps.MessageQueue.Producer
{
    public class RabbitMqProducer : IProducer, IDisposable
    {
        private readonly IBusControl _bus;
        private bool _isStarted;

        public RabbitMqProducer(RabbitMQConfig configuration)
        {
            _bus = ConfigureBus(configuration);
        }

        public async Task Publish(object message)
        {
            if (_isStarted == false)
            {
                await _bus.StartAsync();
                _isStarted = true;
            }

            _bus.Publish(message).GetAwaiter().GetResult();
        }

        public void Stop()
        {
            _bus.Stop();
            _isStarted = false;
        }

        public void Dispose() => Stop();

        private static IBusControl ConfigureBus(RabbitMQConfig configuration)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(configuration.Uri), h =>
                {
                    h.Username(configuration.Username);
                    h.Password(configuration.Password);
                });
            });
        }
    }
}
