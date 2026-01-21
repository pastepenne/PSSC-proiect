namespace Orders.Domain.Models
{
    // Reprezinta un item din comanda, nevalidat (vine din input)
    public record UnvalidatedOrderItem(string ProductCode, int Quantity);
}
