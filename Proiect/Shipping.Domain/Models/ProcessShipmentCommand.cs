namespace Shipping.Domain.Models
{
    public record ProcessShipmentCommand(
        string OrderNumber,
        string InvoiceNumber,
        string ClientEmail,
        string ShippingAddress,
        decimal TotalAmount,
        IReadOnlyCollection<ShipmentItem> Items);
}
