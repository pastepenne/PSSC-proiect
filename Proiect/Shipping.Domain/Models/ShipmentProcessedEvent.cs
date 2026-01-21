using static Shipping.Domain.Models.Shipment;

namespace Shipping.Domain.Models
{
    public static class ShipmentProcessedEvent
    {
        public interface IShipmentProcessedEvent { }

        public record ShipmentProcessSucceededEvent : IShipmentProcessedEvent
        {
            public string TrackingNumber { get; }
            public string OrderNumber { get; }
            public string InvoiceNumber { get; }
            public DateTime ShippedDate { get; }
            public string CourierName { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }

            internal ShipmentProcessSucceededEvent(string trackingNumber, string orderNumber, 
                string invoiceNumber, DateTime shippedDate, string courierName, 
                string clientEmail, string shippingAddress)
            {
                TrackingNumber = trackingNumber;
                OrderNumber = orderNumber;
                InvoiceNumber = invoiceNumber;
                ShippedDate = shippedDate;
                CourierName = courierName;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
            }
        }

        public record ShipmentProcessFailedEvent : IShipmentProcessedEvent
        {
            public string OrderNumber { get; }
            public string Reason { get; }

            internal ShipmentProcessFailedEvent(string orderNumber, string reason)
            {
                OrderNumber = orderNumber;
                Reason = reason;
            }
        }

        public static IShipmentProcessedEvent ToEvent(this IShipment shipment) =>
            shipment switch
            {
                UnprocessedShipment _ => new ShipmentProcessFailedEvent("", "Unexpected unprocessed state"),
                PreparedShipment _ => new ShipmentProcessFailedEvent("", "Unexpected prepared state"),
                FailedShipment failed => new ShipmentProcessFailedEvent(failed.OrderNumber, failed.Reason),
                ShippedShipment shipped => new ShipmentProcessSucceededEvent(
                    shipped.TrackingNumber,
                    shipped.OrderNumber,
                    shipped.InvoiceNumber,
                    shipped.ShippedDate,
                    shipped.CourierName,
                    shipped.ClientEmail,
                    shipped.ShippingAddress),
                _ => throw new NotImplementedException()
            };
    }
}
