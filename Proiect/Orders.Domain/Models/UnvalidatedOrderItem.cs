namespace Orders.Domain.Models
{
    // Reprezintă un item din comandă, nevalidat (vine din input)
    public record UnvalidatedOrderItem(string ProductCode, int Quantity);
}
