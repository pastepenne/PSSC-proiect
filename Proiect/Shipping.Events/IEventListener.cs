namespace Shipping.Events
{
    public interface IEventListener
    {
        Task StartAsync(string topicName, string subscriptionName, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
