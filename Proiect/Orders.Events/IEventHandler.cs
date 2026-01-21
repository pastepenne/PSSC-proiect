using CloudNative.CloudEvents;
using Orders.Events.Models;

namespace Orders.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }
        Task<EventProcessingResult> HandleAsync(CloudEvent cloudEvent);
    }
}
