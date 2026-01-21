using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Domain.Operations
{
    internal sealed class GenerateInvoiceOperation : InvoiceOperation
    {
        protected override IInvoice OnUnprocessed(UnprocessedInvoice unprocessed)
        {
            // Generam numarul facturii
            string invoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            return new GeneratedInvoice(
                invoiceNumber,
                unprocessed.OrderNumber,
                unprocessed.OrderDate,
                DateTime.UtcNow,
                unprocessed.ClientEmail,
                unprocessed.ShippingAddress,
                unprocessed.TotalAmount,
                unprocessed.Items);
        }
    }
}
