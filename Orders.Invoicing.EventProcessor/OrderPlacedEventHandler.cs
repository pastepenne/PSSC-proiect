using Invoicing.Domain.Models;
using Invoicing.Domain.Workflows;
using Invoicing.Dto.Events;
using Invoicing.Dto.Models;
using Microsoft.Extensions.Logging;
using Orders.Dto.Events;
using Orders.Events;
using InvEvents = Invoicing.Events;
using static Invoicing.Domain.Models.InvoiceProcessedEvent;

namespace Orders.Invoicing.EventProcessor
{
    internal class OrderPlacedEventHandler : Orders.Events.AbstractEventHandler<OrderPlacedEvent>
    {
        private readonly ProcessInvoiceWorkflow processInvoiceWorkflow;
        private readonly InvEvents.IEventSender eventSender;
        private readonly ILogger<OrderPlacedEventHandler> logger;

        public OrderPlacedEventHandler(
            ProcessInvoiceWorkflow processInvoiceWorkflow,
            InvEvents.IEventSender eventSender,
            ILogger<OrderPlacedEventHandler> logger)
        {
            this.processInvoiceWorkflow = processInvoiceWorkflow;
            this.eventSender = eventSender;
            this.logger = logger;
        }

        public override string[] EventTypes => new[] { nameof(OrderPlacedEvent) };

        protected override async Task<Orders.Events.Models.EventProcessingResult> OnHandleAsync(OrderPlacedEvent eventData)
        {
            logger.LogInformation("Received OrderPlacedEvent for order {OrderNumber}", eventData.OrderNumber);
            Console.WriteLine(eventData.ToString());

            // Creem comanda pentru procesare
            var command = new ProcessInvoiceCommand(
                eventData.OrderNumber,
                eventData.PlacedDate,
                eventData.ClientEmail,
                eventData.ShippingAddress,
                eventData.TotalPrice,
                eventData.Items.Select(i => new InvoiceItem(i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList().AsReadOnly());

            // Executăm workflow-ul
            IInvoiceProcessedEvent result = await processInvoiceWorkflow.ExecuteAsync(command);

            // Dacă a reușit, trimitem event către Shipping
            if (result is InvoiceProcessSucceededEvent successEvent)
            {
                logger.LogInformation("Invoice {InvoiceNumber} created for order {OrderNumber}", 
                    successEvent.InvoiceNumber, successEvent.OrderNumber);

                var invoiceSentEvent = new InvoiceSentEvent
                {
                    InvoiceNumber = successEvent.InvoiceNumber,
                    OrderNumber = successEvent.OrderNumber,
                    InvoiceDate = successEvent.InvoiceDate,
                    SentDate = successEvent.SentDate,
                    ClientEmail = successEvent.ClientEmail,
                    ShippingAddress = successEvent.ShippingAddress,
                    TotalAmount = successEvent.TotalAmount,
                    Items = successEvent.Items.Select(i => new InvoiceItemDto
                    {
                        ProductCode = i.ProductCode,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                };

                await eventSender.SendAsync("invoices", invoiceSentEvent);
                logger.LogInformation("InvoiceSentEvent sent to Service Bus for order {OrderNumber}", successEvent.OrderNumber);

                return Orders.Events.Models.EventProcessingResult.Completed;
            }
            else if (result is InvoiceProcessFailedEvent failedEvent)
            {
                logger.LogError("Invoice processing failed for order {OrderNumber}: {Reason}", 
                    failedEvent.OrderNumber, failedEvent.Reason);
                return Orders.Events.Models.EventProcessingResult.Failed;
            }

            return Orders.Events.Models.EventProcessingResult.Completed;
        }
    }
}
