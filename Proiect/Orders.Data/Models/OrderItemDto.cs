namespace Orders.Data.Models
{
    // DTO pentru tabelul OrderItem
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        public OrderDto? Order { get; set; }
    }
}
