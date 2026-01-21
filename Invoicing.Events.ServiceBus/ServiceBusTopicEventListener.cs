using Azure.Messaging.ServiceBus;
using CloudNative.CloudEvents.SystemTextJson;
using Microsoft.Extensions.Logging;
using Invoicing.Events.Models;

namespace Invoicing.Events.ServiceBus
{
    public class ServiceBusTopicEventListener : IEventListener
    {
        private ServiceBusProcessor? processor;
        private readonly ServiceBusClient client;
        private readonly Dictionary<string, IEventHandler> eventHandlers;
        private readonly ILogger<ServiceBusTopicEventListener> logger;
        private readonly JsonEventFormatter formatter = new();

        public ServiceBusTopicEventListener(ServiceBusClient client, ILogger<ServiceBusTopicEventListener> logger, IEnumerable<IEventHandler> eventHandlers)
        {
            this.client = client;
            this.eventHandlers = eventHandlers.SelectMany(handler => handler.EventTypes
                                                                          .Select(eventType => (eventType, handler)))
                                                                          .ToDictionary(pair => pair.eventType, pair => pair.handler);
            this.logger = logger;
        }

        public Task StartAsync(string topicName, string subscriptionName, CancellationToken cancellationToken)
        {
            ServiceBusProcessorOptions options = new()
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 2
            };
            processor = client.CreateProcessor(topicName, subscriptionName, options);
            processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
            return processor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (processor != null)
            {
                await processor.StopProcessingAsync(cancellationToken);
                processor.ProcessMessageAsync -= Processor_ProcessMessageAsync;
                processor.ProcessErrorAsync -= Processor_ProcessErrorAsync;
            }
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            logger.LogError(arg.Exception, "{ErrorSource}, {FullyQualifiedNamespace}, {EntityPath}",
                arg.ErrorSource, arg.FullyQualifiedNamespace, arg.EntityPath);
            return Task.CompletedTask;
        }

        private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            if (await EnsureMaxDeliveryCountAsync(arg))
            {
                await ProcessMessageAsCloudEventAsync(arg);
            }
        }

        private async Task<bool> EnsureMaxDeliveryCountAsync(ProcessMessageEventArgs arg)
        {
            bool canContinue = true;
            if (arg.Message.DeliveryCount > 5)
            {
                logger.LogError("Retry count exceeded {MessageId}", arg.Message.MessageId);
                await arg.DeadLetterMessageAsync(arg.Message, "Retry count exceeded");
                canContinue = false;
            }
            return canContinue;
        }

        private async Task ProcessMessageAsCloudEventAsync(ProcessMessageEventArgs arg)
        {
            BinaryData data = arg.Message.Body;
            CloudNative.CloudEvents.CloudEvent cloudEvent = formatter.DecodeStructuredModeMessage(data.ToStream(), null, null);

            if (eventHandlers.TryGetValue(cloudEvent.Type!, out IEventHandler? handler))
            {
                EventProcessingResult result = await InvokeHandlerAsync(cloudEvent, handler);
                await InterpretResult(result, arg);
            }
            else
            {
                logger.LogError("No handler found for {EventType}", cloudEvent.Type);
                await arg.CompleteMessageAsync(arg.Message);
            }
        }

        private async Task<EventProcessingResult> InvokeHandlerAsync(CloudNative.CloudEvents.CloudEvent cloudEvent, IEventHandler handler)
        {
            try
            {
                return await handler.HandleAsync(cloudEvent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling event {EventType}", cloudEvent.Type);
                return EventProcessingResult.Failed;
            }
        }

        private async Task InterpretResult(EventProcessingResult result, ProcessMessageEventArgs arg)
        {
            switch (result)
            {
                case EventProcessingResult.Completed:
                    await arg.CompleteMessageAsync(arg.Message);
                    break;
                case EventProcessingResult.Failed:
                    await arg.AbandonMessageAsync(arg.Message);
                    break;
            }
        }
    }
}
