using Invoicing.Data.Models;
using Invoicing.Domain.Repositories;
using static Invoicing.Domain.Models.Invoice;

namespace Invoicing.Data.Repositories
{
    public class InvoicesRepository : IInvoicesRepository
    {
        private readonly InvoicingContext dbContext;

        public InvoicesRepository(InvoicingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveInvoiceAsync(SentInvoice invoice)
        {
            var invoiceDto = new InvoiceDto
            {
                InvoiceNumber = invoice.InvoiceNumber,
                OrderNumber = invoice.OrderNumber,
                OrderDate = invoice.OrderDate,
                InvoiceDate = invoice.InvoiceDate,
                SentDate = invoice.SentDate,
                ClientEmail = invoice.ClientEmail,
                ShippingAddress = invoice.ShippingAddress,
                TotalAmount = invoice.TotalAmount,
                Items = invoice.Items.Select(item => new InvoiceItemDto
                {
                    ProductCode = item.ProductCode,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            dbContext.Invoices.Add(invoiceDto);
            await dbContext.SaveChangesAsync();
        }
    }
}
