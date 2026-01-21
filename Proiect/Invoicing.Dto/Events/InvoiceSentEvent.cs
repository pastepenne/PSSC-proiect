using Invoicing.Dto.Models;
using System.Text;

namespace Invoicing.Dto.Events
{
    // Evenimentul trimis către Shipping cand o factură a fost procesata
    public record InvoiceSentEvent
    {
        public string InvoiceNumber { get; init; } = string.Empty;
        public string OrderNumber { get; init; } = string.Empty;
        public DateTime InvoiceDate { get; init; }
        public DateTime SentDate { get; init; }
        public string ClientEmail { get; init; } = string.Empty;
        public string ShippingAddress { get; init; } = string.Empty;
        public decimal TotalAmount { get; init; }
        public List<InvoiceItemDto> Items { get; init; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine("===== INVOICE SENT EVENT =====");
            sb.AppendLine($"Invoice Number: {InvoiceNumber}");
            sb.AppendLine($"Order Number: {OrderNumber}");
            sb.AppendLine($"Invoice Date: {InvoiceDate}");
            sb.AppendLine($"Sent Date: {SentDate}");
            sb.AppendLine($"Client Email: {ClientEmail}");
            sb.AppendLine($"Shipping Address: {ShippingAddress}");
            sb.AppendLine($"Total Amount: {TotalAmount:F2} RON");
            sb.AppendLine("Items:");
            foreach (var item in Items)
            {
                sb.AppendLine($"  - {item.ProductCode}: {item.Quantity} x {item.UnitPrice:F2} = {item.TotalPrice:F2} RON");
            }
            sb.AppendLine("==============================");
            return sb.ToString();
        }
    }
}
