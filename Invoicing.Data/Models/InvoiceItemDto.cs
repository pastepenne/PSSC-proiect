namespace Invoicing.Data.Models
{
    public class InvoiceItemDto
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public InvoiceDto? Invoice { get; set; }
    }
}
