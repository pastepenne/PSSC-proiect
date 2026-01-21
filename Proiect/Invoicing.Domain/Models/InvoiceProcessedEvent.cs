using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Domain.Models
{
    public static class InvoiceProcessedEvent
    {
        public interface IInvoiceProcessedEvent { }

        public record InvoiceProcessSucceededEvent : IInvoiceProcessedEvent
        {
            public string InvoiceNumber { get; }
            public string OrderNumber { get; }
            public DateTime InvoiceDate { get; }
            public DateTime SentDate { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IEnumerable<InvoiceItem> Items { get; }

            internal InvoiceProcessSucceededEvent(string invoiceNumber, string orderNumber, 
                DateTime invoiceDate, DateTime sentDate, string clientEmail, 
                string shippingAddress, decimal totalAmount, IEnumerable<InvoiceItem> items)
            {
                InvoiceNumber = invoiceNumber;
                OrderNumber = orderNumber;
                InvoiceDate = invoiceDate;
                SentDate = sentDate;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }
        }

        public record InvoiceProcessFailedEvent : IInvoiceProcessedEvent
        {
            public string OrderNumber { get; }
            public string Reason { get; }

            internal InvoiceProcessFailedEvent(string orderNumber, string reason)
            {
                OrderNumber = orderNumber;
                Reason = reason;
            }
        }

        public static IInvoiceProcessedEvent ToEvent(this IInvoice invoice) =>
            invoice switch
            {
                UnprocessedInvoice _ => new InvoiceProcessFailedEvent("", "Unexpected unprocessed state"),
                GeneratedInvoice _ => new InvoiceProcessFailedEvent("", "Unexpected generated state"),
                FailedInvoice failed => new InvoiceProcessFailedEvent(failed.OrderNumber, failed.Reason),
                SentInvoice sent => new InvoiceProcessSucceededEvent(
                    sent.InvoiceNumber,
                    sent.OrderNumber,
                    sent.InvoiceDate,
                    sent.SentDate,
                    sent.ClientEmail,
                    sent.ShippingAddress,
                    sent.TotalAmount,
                    sent.Items),
                _ => throw new NotImplementedException()
            };
    }
}
