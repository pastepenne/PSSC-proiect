namespace Shipping.Domain.Models
{
    public record ShipmentItem(string ProductCode, int Quantity, decimal UnitPrice, decimal TotalPrice);
}
