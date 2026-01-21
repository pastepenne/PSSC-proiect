using Invoicing.Events;
using Microsoft.Extensions.Hosting;

namespace Invoicing.Shipping.EventProcessor
{
    internal class Worker : IHostedService
    {
        private readonly IEventListener eventListener;

        public Worker(IEventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            Console.WriteLine("   SHIPPING EVENT PROCESSOR STARTED");
            Console.WriteLine("   Listening for invoices on 'invoices' topic");
            return eventListener.StartAsync("invoices", "shipping", cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Shipping Worker stopped!");
            return eventListener.StopAsync(cancellationToken);
        }
    }
}
