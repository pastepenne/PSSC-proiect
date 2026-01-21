namespace Invoicing.Domain.Models
{
    public record InvoiceItem(string ProductCode, int Quantity, decimal UnitPrice, decimal TotalPrice);
}
