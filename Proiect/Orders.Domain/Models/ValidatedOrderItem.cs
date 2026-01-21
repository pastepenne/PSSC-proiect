namespace Orders.Domain.Models
{
    // Reprezinta un item din comanda, validat
    public record ValidatedOrderItem(ProductCode ProductCode, Quantity Quantity);
}
