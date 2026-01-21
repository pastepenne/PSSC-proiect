using Microsoft.Extensions.Logging;
using static Shipping.Domain.Models.Shipment;

namespace Shipping.Domain.Operations
{
    internal sealed class ShipToCarrierOperation : ShipmentOperation
    {
        private readonly ILogger logger;
        private static readonly string[] Couriers = { "DHL", "FedEx", "UPS", "FAN Courier", "Cargus" };
        private static readonly Random random = new();

        public ShipToCarrierOperation(ILogger logger)
        {
            this.logger = logger;
        }

        protected override IShipment OnPrepared(PreparedShipment prepared)
        {
            // Selectam un curier aleatoriu (simulare)
            string courierName = Couriers[random.Next(Couriers.Length)];

            logger.LogInformation("Shipment {TrackingNumber} handed over to courier {CourierName}", 
                prepared.TrackingNumber, courierName);
            logger.LogInformation("Destination: {ShippingAddress}", prepared.ShippingAddress);

            return new ShippedShipment(
                prepared.TrackingNumber,
                prepared.OrderNumber,
                prepared.InvoiceNumber,
                prepared.PreparedDate,
                DateTime.UtcNow,
                courierName,
                prepared.ClientEmail,
                prepared.ShippingAddress,
                prepared.TotalAmount,
                prepared.Items);
        }
    }
}
