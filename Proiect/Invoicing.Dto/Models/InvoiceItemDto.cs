namespace Invoicing.Dto.Models
{
    public record InvoiceItemDto
    {
        public string ProductCode { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init; }
    }
}
