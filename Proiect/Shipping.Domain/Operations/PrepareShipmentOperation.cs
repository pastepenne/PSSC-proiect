using static Shipping.Domain.Models.Shipment;

namespace Shipping.Domain.Operations
{
    internal sealed class PrepareShipmentOperation : ShipmentOperation
    {
        protected override IShipment OnUnprocessed(UnprocessedShipment unprocessed)
        {
            // Generam tracking number
            string trackingNumber = $"TRK-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            return new PreparedShipment(
                trackingNumber,
                unprocessed.OrderNumber,
                unprocessed.InvoiceNumber,
                DateTime.UtcNow,
                unprocessed.ClientEmail,
                unprocessed.ShippingAddress,
                unprocessed.TotalAmount,
                unprocessed.Items);
        }
    }
}
