namespace Invoicing.Domain.Models
{
    public record ProcessInvoiceCommand(
        string OrderNumber,
        DateTime OrderDate,
        string ClientEmail,
        string ShippingAddress,
        decimal TotalAmount,
        IReadOnlyCollection<InvoiceItem> Items);
}
