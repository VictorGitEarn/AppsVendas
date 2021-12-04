namespace Apps.MessageQueue.Producer
{
    public interface IProducer
    {
        Task Publish(object message);
    }
}
