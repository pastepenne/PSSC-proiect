using Microsoft.Extensions.Logging;
using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Domain.Operations
{
    internal sealed class SendInvoiceOperation : InvoiceOperation
    {
        private readonly ILogger logger;

        public SendInvoiceOperation(ILogger logger)
        {
            this.logger = logger;
        }

        protected override IInvoice OnGenerated(GeneratedInvoice generated)
        {
            // Simulam trimiterea facturii pe email
            logger.LogInformation("Sending invoice {InvoiceNumber} to {ClientEmail}", 
                generated.InvoiceNumber, generated.ClientEmail);

            // In realitate, aici ar fi codul pentru trimiterea email-ului
            // Pentru simulare, doar logăm

            return new SentInvoice(
                generated.InvoiceNumber,
                generated.OrderNumber,
                generated.OrderDate,
                generated.InvoiceDate,
                DateTime.UtcNow,
                generated.ClientEmail,
                generated.ShippingAddress,
                generated.TotalAmount,
                generated.Items);
        }
    }
}
