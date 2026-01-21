using Microsoft.Extensions.Logging;
using Shipping.Domain.Models;
using Shipping.Domain.Operations;
using Shipping.Domain.Repositories;
using static Shipping.Domain.Models.Shipment;
using static Shipping.Domain.Models.ShipmentProcessedEvent;

namespace Shipping.Domain.Workflows
{
    public class ProcessShipmentWorkflow
    {
        private readonly IShipmentsRepository shipmentsRepository;
        private readonly ILogger<ProcessShipmentWorkflow> logger;

        public ProcessShipmentWorkflow(IShipmentsRepository shipmentsRepository, ILogger<ProcessShipmentWorkflow> logger)
        {
            this.shipmentsRepository = shipmentsRepository;
            this.logger = logger;
        }

        public async Task<IShipmentProcessedEvent> ExecuteAsync(ProcessShipmentCommand command)
        {
            try
            {
                logger.LogInformation("Processing shipment for order {OrderNumber}", command.OrderNumber);

                // Executam logica de business (pure functions)
                IShipment shipment = ExecuteBusinessLogic(command);

                // Salvam in baza de date DOAR daca coletul a fost expediat
                if (shipment is ShippedShipment shippedShipment)
                {
                    await shipmentsRepository.SaveShipmentAsync(shippedShipment);
                    logger.LogInformation("Shipment {TrackingNumber} saved to database", shippedShipment.TrackingNumber);
                }

                // Returnam evenimentul corespunzator starii
                return shipment.ToEvent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing shipment for order {OrderNumber}", command.OrderNumber);
                return new ShipmentProcessFailedEvent(command.OrderNumber, "Unexpected error occurred while processing shipment.");
            }
        }

        private IShipment ExecuteBusinessLogic(ProcessShipmentCommand command)
        {
            // Starea initiala - coletul neprocesat
            UnprocessedShipment unprocessedShipment = new(
                command.OrderNumber,
                command.InvoiceNumber,
                command.ClientEmail,
                command.ShippingAddress,
                command.TotalAmount,
                command.Items);

            // Transform chain
            IShipment shipment = new PrepareShipmentOperation().Transform(unprocessedShipment);
            shipment = new ShipToCarrierOperation(logger).Transform(shipment);

            return shipment;
        }
    }
}
