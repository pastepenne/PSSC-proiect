using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using Orders.Data;
using Orders.Data.Repositories;
using Orders.Domain.Repositories;
using Orders.Domain.Workflows;
using Orders.Events;
using Orders.Events.ServiceBus;

namespace Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Adaugam servicii in containerul de dependinte (DI container)
            
            // Add DbContext
            builder.Services.AddDbContext<OrdersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Repositories
            builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
            builder.Services.AddTransient<IOrdersRepository, OrdersRepository>();

            // Add Workflow
            builder.Services.AddTransient<PlaceOrderWorkflow>();

            // Add Event Sender (Service Bus)
            builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            builder.Services.AddAzureClients(client =>
            {
                client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
            });

            // Add Controllers
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Orders API", 
                    Version = "v1",
                    Description = "E-Commerce Orders API - First context (client facing)"
                });
            });

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
