namespace Orders.Dto.Models
{
    public record OrderItemDto
    {
        public string ProductCode { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init; }
    }
}
