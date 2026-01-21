namespace Orders.Domain.Models
{
    // Reprezintă un item din comandă, validat
    public record ValidatedOrderItem(ProductCode ProductCode, Quantity Quantity);
}
