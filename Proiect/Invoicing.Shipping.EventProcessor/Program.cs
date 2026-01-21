using Invoicing.Events;
using Invoicing.Events.ServiceBus;
using Invoicing.Shipping.EventProcessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shipping.Data;
using Shipping.Data.Repositories;
using Shipping.Domain.Repositories;
using Shipping.Domain.Workflows;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Adaugam DbContext pentru Shipping
                services.AddDbContext<ShippingContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                // Adaugam Repositories
                services.AddTransient<IShipmentsRepository, ShipmentsRepository>();

                // Adaugam Workflow
                services.AddTransient<ProcessShipmentWorkflow>();

                // Adaugam Azure Service Bus client
                services.AddAzureClients(builder =>
                {
                    builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                });

                // Adaugam Event Listener (pentru receptia de la Invoice)
                services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
                services.AddSingleton<IEventHandler, InvoiceSentEventHandler>();

                // Adaugam Worker
                services.AddHostedService<Worker>();
            });
}
