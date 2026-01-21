namespace Orders.Data.Models
{
    // DTO pentru tabelul Order
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime PlacedDate { get; set; }
        
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
