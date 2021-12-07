using MassTransit.RabbitMqTransport;

namespace Apps.MessageQueue
{
    public interface IConsumerApps
    {
        void Attach(IRabbitMqBusFactoryConfigurator cfg);
    }
}
