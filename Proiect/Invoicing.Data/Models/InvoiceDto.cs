namespace Invoicing.Data.Models
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime SentDate { get; set; }
        public string ClientEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
