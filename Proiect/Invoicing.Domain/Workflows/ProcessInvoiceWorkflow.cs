using Microsoft.Extensions.Logging;
using Invoicing.Domain.Models;
using Invoicing.Domain.Operations;
using Invoicing.Domain.Repositories;
using static Invoicing.Domain.Models.Invoice;
using static Invoicing.Domain.Models.InvoiceProcessedEvent;

namespace Invoicing.Domain.Workflows
{
    public class ProcessInvoiceWorkflow
    {
        private readonly IInvoicesRepository invoicesRepository;
        private readonly ILogger<ProcessInvoiceWorkflow> logger;

        public ProcessInvoiceWorkflow(IInvoicesRepository invoicesRepository, ILogger<ProcessInvoiceWorkflow> logger)
        {
            this.invoicesRepository = invoicesRepository;
            this.logger = logger;
        }

        public async Task<IInvoiceProcessedEvent> ExecuteAsync(ProcessInvoiceCommand command)
        {
            try
            {
                logger.LogInformation("Processing invoice for order {OrderNumber}", command.OrderNumber);

                // Executam logica de business (pure functions)
                IInvoice invoice = ExecuteBusinessLogic(command);

                // Salvam în baza de date DOAR dacă factura a fost trimisa cu succes
                if (invoice is SentInvoice sentInvoice)
                {
                    await invoicesRepository.SaveInvoiceAsync(sentInvoice);
                    logger.LogInformation("Invoice {InvoiceNumber} saved to database", sentInvoice.InvoiceNumber);
                }

                // Returnam evenimentul corespunzător stării
                return invoice.ToEvent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing invoice for order {OrderNumber}", command.OrderNumber);
                return new InvoiceProcessFailedEvent(command.OrderNumber, "Unexpected error occurred while processing invoice.");
            }
        }

        private IInvoice ExecuteBusinessLogic(ProcessInvoiceCommand command)
        {
            // Starea initială - factura neprocesată
            UnprocessedInvoice unprocessedInvoice = new(
                command.OrderNumber,
                command.OrderDate,
                command.ClientEmail,
                command.ShippingAddress,
                command.TotalAmount,
                command.Items);

            // Transformam chain
            IInvoice invoice = new GenerateInvoiceOperation().Transform(unprocessedInvoice);
            invoice = new SendInvoiceOperation(logger).Transform(invoice);

            return invoice;
        }
    }
}
