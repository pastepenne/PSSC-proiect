using Microsoft.AspNetCore.Mvc;
using Orders.Api.Models;
using Orders.Domain.Models;
using Orders.Domain.Workflows;
using Orders.Dto.Models;
using Orders.Events;
using static Orders.Domain.Models.OrderPlacedEvent;
using DtoOrderPlacedEvent = Orders.Dto.Events.OrderPlacedEvent;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> logger;
        private readonly PlaceOrderWorkflow placeOrderWorkflow;
        private readonly IEventSender eventSender;

        public OrdersController(
            ILogger<OrdersController> logger,
            PlaceOrderWorkflow placeOrderWorkflow,
            IEventSender eventSender)
        {
            this.logger = logger;
            this.placeOrderWorkflow = placeOrderWorkflow;
            this.eventSender = eventSender;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] InputOrder inputOrder)
        {
            logger.LogInformation("Received order request from {ClientEmail}", inputOrder.ClientEmail);

            // Mapare din InputOrder în UnvalidatedOrderItem
            var unvalidatedItems = inputOrder.Items
                .Select(item => new UnvalidatedOrderItem(item.ProductCode, item.Quantity))
                .ToList()
                .AsReadOnly();

            // Creare comandă
            PlaceOrderCommand command = new(unvalidatedItems, inputOrder.ClientEmail, inputOrder.ShippingAddress);

            // Executare workflow
            IOrderPlacedEvent workflowResult = await placeOrderWorkflow.ExecuteAsync(command);

            // Interpretare rezultat
            IActionResult response = workflowResult switch
            {
                OrderPlaceSucceededEvent @event => await HandleSuccess(@event),
                OrderPlaceFailedEvent @event => HandleFailure(@event),
                _ => throw new NotImplementedException()
            };

            return response;
        }

        private async Task<IActionResult> HandleSuccess(OrderPlaceSucceededEvent successEvent)
        {
            logger.LogInformation("Order {OrderNumber} placed successfully", successEvent.OrderNumber);

            // Trimitem event către Invoicing prin Service Bus
            var orderPlacedEvent = new DtoOrderPlacedEvent
            {
                OrderNumber = successEvent.OrderNumber,
                PlacedDate = successEvent.PlacedDate,
                ClientEmail = successEvent.ClientEmail,
                ShippingAddress = successEvent.ShippingAddress,
                TotalPrice = successEvent.TotalPrice,
                Items = successEvent.OrderItems.Select(item => new OrderItemDto
                {
                    ProductCode = item.ProductCode.Value,
                    Quantity = item.Quantity.Value,
                    UnitPrice = item.UnitPrice.Value,
                    TotalPrice = item.TotalPrice.Value
                }).ToList()
            };

            await eventSender.SendAsync("orders", orderPlacedEvent);
            logger.LogInformation("Order event sent to Service Bus for order {OrderNumber}", successEvent.OrderNumber);

            return Ok(new
            {
                Message = "Order placed successfully",
                OrderNumber = successEvent.OrderNumber,
                TotalPrice = successEvent.TotalPrice,
                PlacedDate = successEvent.PlacedDate
            });
        }

        private IActionResult HandleFailure(OrderPlaceFailedEvent failedEvent)
        {
            logger.LogWarning("Order placement failed: {Reasons}", string.Join(", ", failedEvent.Reasons));
            return BadRequest(new
            {
                Message = "Order placement failed",
                Errors = failedEvent.Reasons
            });
        }
    }
}