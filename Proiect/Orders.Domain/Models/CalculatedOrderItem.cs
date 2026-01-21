namespace Orders.Domain.Models
{
    // Reprezintă un item din comandă, cu preț calculat
    public record CalculatedOrderItem(ProductCode ProductCode, Quantity Quantity, Price UnitPrice, Price TotalPrice)
    {
        public int OrderItemId { get; init; }
    }
}
