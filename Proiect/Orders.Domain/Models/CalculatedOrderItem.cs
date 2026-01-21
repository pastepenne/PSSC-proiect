namespace Orders.Domain.Models
{
    // Reprezinta un item din comanda, cu pret calculat
    public record CalculatedOrderItem(ProductCode ProductCode, Quantity Quantity, Price UnitPrice, Price TotalPrice)
    {
        public int OrderItemId { get; init; }
    }
}
