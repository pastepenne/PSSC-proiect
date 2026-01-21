using CloudNative.CloudEvents;
using Shipping.Events.Models;

namespace Shipping.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }
        Task<EventProcessingResult> HandleAsync(CloudEvent cloudEvent);
    }
}
