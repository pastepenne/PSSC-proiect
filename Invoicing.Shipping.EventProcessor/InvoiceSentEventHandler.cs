using Invoicing.Dto.Events;
using Microsoft.Extensions.Logging;
using Shipping.Domain.Models;
using Shipping.Domain.Workflows;
using Shipping.Events;
using Shipping.Events.Models;
using static Shipping.Domain.Models.ShipmentProcessedEvent;

namespace Invoicing.Shipping.EventProcessor
{
    internal class InvoiceSentEventHandler : Invoicing.Events.AbstractEventHandler<InvoiceSentEvent>
    {
        private readonly ProcessShipmentWorkflow processShipmentWorkflow;
        private readonly ILogger<InvoiceSentEventHandler> logger;

        public InvoiceSentEventHandler(
            ProcessShipmentWorkflow processShipmentWorkflow,
            ILogger<InvoiceSentEventHandler> logger)
        {
            this.processShipmentWorkflow = processShipmentWorkflow;
            this.logger = logger;
        }

        public override string[] EventTypes => new[] { nameof(InvoiceSentEvent) };

        protected override async Task<Invoicing.Events.Models.EventProcessingResult> OnHandleAsync(InvoiceSentEvent eventData)
        {
            logger.LogInformation("Received InvoiceSentEvent for order {OrderNumber}, invoice {InvoiceNumber}", 
                eventData.OrderNumber, eventData.InvoiceNumber);
            Console.WriteLine(eventData.ToString());

            // Creem comanda pentru procesare
            var command = new ProcessShipmentCommand(
                eventData.OrderNumber,
                eventData.InvoiceNumber,
                eventData.ClientEmail,
                eventData.ShippingAddress,
                eventData.TotalAmount,
                eventData.Items.Select(i => new ShipmentItem(i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList().AsReadOnly());

            // Executam workflow-ul
            IShipmentProcessedEvent result = await processShipmentWorkflow.ExecuteAsync(command);

            // Logam rezultatul
            if (result is ShipmentProcessSucceededEvent successEvent)
            {
                logger.LogInformation("Shipment {TrackingNumber} created for order {OrderNumber}", 
                    successEvent.TrackingNumber, successEvent.OrderNumber);
                logger.LogInformation("Courier: {CourierName}, Destination: {ShippingAddress}", 
                    successEvent.CourierName, successEvent.ShippingAddress);
                
                Console.WriteLine();
                Console.WriteLine("===== SHIPMENT COMPLETED =====");
                Console.WriteLine($"Tracking Number: {successEvent.TrackingNumber}");
                Console.WriteLine($"Order Number: {successEvent.OrderNumber}");
                Console.WriteLine($"Invoice Number: {successEvent.InvoiceNumber}");
                Console.WriteLine($"Courier: {successEvent.CourierName}");
                Console.WriteLine($"Shipped Date: {successEvent.ShippedDate}");
                Console.WriteLine($"Destination: {successEvent.ShippingAddress}");
                Console.WriteLine("==============================");
                Console.WriteLine();

                return Invoicing.Events.Models.EventProcessingResult.Completed;
            }
            else if (result is ShipmentProcessFailedEvent failedEvent)
            {
                logger.LogError("Shipment processing failed for order {OrderNumber}: {Reason}", 
                    failedEvent.OrderNumber, failedEvent.Reason);
                return Invoicing.Events.Models.EventProcessingResult.Failed;
            }

            return Invoicing.Events.Models.EventProcessingResult.Completed;
        }
    }
}
