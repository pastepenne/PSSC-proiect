﻿using Invoicing.Data;
using Invoicing.Data.Repositories;
using Invoicing.Domain.Repositories;
using Invoicing.Domain.Workflows;
using Invoicing.Events.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Events;
using Orders.Events.ServiceBus;
using Orders.Invoicing.EventProcessor;

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
                // Add DbContext for Invoicing
                services.AddDbContext<InvoicingContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                // Add Repositories
                services.AddTransient<IInvoicesRepository, InvoicesRepository>();

                // Add Workflow
                services.AddTransient<ProcessInvoiceWorkflow>();

                // Add Azure Service Bus client
                services.AddAzureClients(builder =>
                {
                    builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                });

                // Add Event Listener (for receiving from Orders)
                services.AddSingleton<IEventListener, Orders.Events.ServiceBus.ServiceBusTopicEventListener>();
                services.AddSingleton<IEventHandler, OrderPlacedEventHandler>();

                // Add Event Sender (for sending to Shipping)
                services.AddSingleton<Invoicing.Events.IEventSender, Invoicing.Events.ServiceBus.ServiceBusTopicEventSender>();

                // Add Worker
                services.AddHostedService<Worker>();
            });
}
