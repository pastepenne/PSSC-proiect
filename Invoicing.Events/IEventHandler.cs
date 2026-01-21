using CloudNative.CloudEvents;
using Invoicing.Events.Models;

namespace Invoicing.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }
        Task<EventProcessingResult> HandleAsync(CloudEvent cloudEvent);
    }
}
