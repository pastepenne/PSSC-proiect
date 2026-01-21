namespace Orders.Events
{
    public interface IEventSender
    {
        Task SendAsync<T>(string topicName, T @event);
    }
}
