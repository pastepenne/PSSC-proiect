using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Domain.Operations
{
    internal abstract class InvoiceOperation
    {
        internal IInvoice Transform(IInvoice invoice) => invoice switch
        {
            UnprocessedInvoice unprocessed => OnUnprocessed(unprocessed),
            GeneratedInvoice generated => OnGenerated(generated),
            SentInvoice sent => OnSent(sent),
            FailedInvoice failed => OnFailed(failed),
            _ => throw new InvalidOperationException($"Invalid invoice state: {invoice.GetType().Name}")
        };

        protected virtual IInvoice OnUnprocessed(UnprocessedInvoice unprocessed) => unprocessed;
        protected virtual IInvoice OnGenerated(GeneratedInvoice generated) => generated;
        protected virtual IInvoice OnSent(SentInvoice sent) => sent;
        protected virtual IInvoice OnFailed(FailedInvoice failed) => failed;
    }
}
