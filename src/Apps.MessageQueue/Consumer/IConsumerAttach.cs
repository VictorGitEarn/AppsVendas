using MassTransit.RabbitMqTransport;

namespace Apps.MessageQueue.Consumer
{
    public interface IConsumerAttach
    {
        void Attach(IRabbitMqBusFactoryConfigurator cfg);
    }
}
