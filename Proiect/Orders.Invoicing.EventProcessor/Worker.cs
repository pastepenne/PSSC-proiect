using Orders.Events;
using Microsoft.Extensions.Hosting;

namespace Orders.Invoicing.EventProcessor
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
            Console.WriteLine("===========================================");
            Console.WriteLine("   INVOICING EVENT PROCESSOR STARTED");
            Console.WriteLine("   Listening for orders on 'orders' topic");
            Console.WriteLine("===========================================");
            return eventListener.StartAsync("orders", "invoicing", cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Invoicing Worker stopped!");
            return eventListener.StopAsync(cancellationToken);
        }
    }
}
