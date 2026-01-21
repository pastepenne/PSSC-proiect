using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Domain.Repositories
{
    public interface IInvoicesRepository
    {
        Task SaveInvoiceAsync(SentInvoice invoice);
    }
}
