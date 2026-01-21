namespace Shipping.Data.Models
{
    public class ShipmentItemDto
    {
        public int ShipmentItemId { get; set; }
        public int ShipmentId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public ShipmentDto? Shipment { get; set; }
    }
}
