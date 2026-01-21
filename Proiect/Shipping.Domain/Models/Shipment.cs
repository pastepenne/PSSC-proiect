namespace Shipping.Domain.Models
{
    public static class Shipment
    {
        public interface IShipment { }

        // Starea initiala - coletul neprocesat
        public record UnprocessedShipment : IShipment
        {
            public UnprocessedShipment(string orderNumber, string invoiceNumber, string clientEmail, 
                string shippingAddress, decimal totalAmount, IReadOnlyCollection<ShipmentItem> items)
            {
                OrderNumber = orderNumber;
                InvoiceNumber = invoiceNumber;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string OrderNumber { get; }
            public string InvoiceNumber { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<ShipmentItem> Items { get; }
        }

        // Starea pregatita - coletul este pregatit pentru expediere
        public record PreparedShipment : IShipment
        {
            internal PreparedShipment(string trackingNumber, string orderNumber, string invoiceNumber, 
                DateTime preparedDate, string clientEmail, string shippingAddress, 
                decimal totalAmount, IReadOnlyCollection<ShipmentItem> items)
            {
                TrackingNumber = trackingNumber;
                OrderNumber = orderNumber;
                InvoiceNumber = invoiceNumber;
                PreparedDate = preparedDate;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string TrackingNumber { get; }
            public string OrderNumber { get; }
            public string InvoiceNumber { get; }
            public DateTime PreparedDate { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<ShipmentItem> Items { get; }
        }

        // Starea finala - coletul a fost predat curierului
        public record ShippedShipment : IShipment
        {
            internal ShippedShipment(string trackingNumber, string orderNumber, string invoiceNumber, 
                DateTime preparedDate, DateTime shippedDate, string courierName, string clientEmail, 
                string shippingAddress, decimal totalAmount, IReadOnlyCollection<ShipmentItem> items)
            {
                TrackingNumber = trackingNumber;
                OrderNumber = orderNumber;
                InvoiceNumber = invoiceNumber;
                PreparedDate = preparedDate;
                ShippedDate = shippedDate;
                CourierName = courierName;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string TrackingNumber { get; }
            public string OrderNumber { get; }
            public string InvoiceNumber { get; }
            public DateTime PreparedDate { get; }
            public DateTime ShippedDate { get; }
            public string CourierName { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<ShipmentItem> Items { get; }
        }

        // Starea de eroare
        public record FailedShipment : IShipment
        {
            internal FailedShipment(string orderNumber, string reason)
            {
                OrderNumber = orderNumber;
                Reason = reason;
            }

            public string OrderNumber { get; }
            public string Reason { get; }
        }
    }
}
