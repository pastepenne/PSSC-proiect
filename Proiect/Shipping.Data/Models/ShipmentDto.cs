namespace Shipping.Data.Models
{
    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime PreparedDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string CourierName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public List<ShipmentItemDto> Items { get; set; } = new();
    }
}
