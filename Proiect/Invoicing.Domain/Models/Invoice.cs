namespace Invoicing.Domain.Models
{
    public static class Invoice
    {
        public interface IInvoice { }

        // Starea initială - factura neprocesata (primita de la Orders)
        public record UnprocessedInvoice : IInvoice
        {
            public UnprocessedInvoice(string orderNumber, DateTime orderDate, string clientEmail, 
                string shippingAddress, decimal totalAmount, IReadOnlyCollection<InvoiceItem> items)
            {
                OrderNumber = orderNumber;
                OrderDate = orderDate;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string OrderNumber { get; }
            public DateTime OrderDate { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<InvoiceItem> Items { get; }
        }

        // Starea procesata - factura generata
        public record GeneratedInvoice : IInvoice
        {
            internal GeneratedInvoice(string invoiceNumber, string orderNumber, DateTime orderDate, 
                DateTime invoiceDate, string clientEmail, string shippingAddress, 
                decimal totalAmount, IReadOnlyCollection<InvoiceItem> items)
            {
                InvoiceNumber = invoiceNumber;
                OrderNumber = orderNumber;
                OrderDate = orderDate;
                InvoiceDate = invoiceDate;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string InvoiceNumber { get; }
            public string OrderNumber { get; }
            public DateTime OrderDate { get; }
            public DateTime InvoiceDate { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<InvoiceItem> Items { get; }
        }

        // Starea finala - factura trimisa pe email
        public record SentInvoice : IInvoice
        {
            internal SentInvoice(string invoiceNumber, string orderNumber, DateTime orderDate, 
                DateTime invoiceDate, DateTime sentDate, string clientEmail, 
                string shippingAddress, decimal totalAmount, IReadOnlyCollection<InvoiceItem> items)
            {
                InvoiceNumber = invoiceNumber;
                OrderNumber = orderNumber;
                OrderDate = orderDate;
                InvoiceDate = invoiceDate;
                SentDate = sentDate;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalAmount = totalAmount;
                Items = items;
            }

            public string InvoiceNumber { get; }
            public string OrderNumber { get; }
            public DateTime OrderDate { get; }
            public DateTime InvoiceDate { get; }
            public DateTime SentDate { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalAmount { get; }
            public IReadOnlyCollection<InvoiceItem> Items { get; }
        }

        // Starea de eroare
        public record FailedInvoice : IInvoice
        {
            internal FailedInvoice(string orderNumber, string reason)
            {
                OrderNumber = orderNumber;
                Reason = reason;
            }

            public string OrderNumber { get; }
            public string Reason { get; }
        }
    }
}
