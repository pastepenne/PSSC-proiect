using Microsoft.Extensions.Logging;
using Orders.Domain.Models;
using Orders.Domain.Operations;
using Orders.Domain.Repositories;
using static Orders.Domain.Models.Order;
using static Orders.Domain.Models.OrderPlacedEvent;

namespace Orders.Domain.Workflows
{
    public class PlaceOrderWorkflow
    {
        private readonly IProductsRepository productsRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly ILogger<PlaceOrderWorkflow> logger;

        public PlaceOrderWorkflow(IProductsRepository productsRepository, IOrdersRepository ordersRepository, ILogger<PlaceOrderWorkflow> logger)
        {
            this.productsRepository = productsRepository;
            this.ordersRepository = ordersRepository;
            this.logger = logger;
        }

        public async Task<IOrderPlacedEvent> ExecuteAsync(PlaceOrderCommand command)
        {
            try
            {
                // 1. Citim din baza de date produsele pentru validare semantică și prețuri
                IEnumerable<string> productCodesToCheck = command.InputOrderItems.Select(item => item.ProductCode);
                List<ExistingProduct> existingProducts = await productsRepository.GetExistingProductsAsync(productCodesToCheck);

                // 2. Executăm logica de business (pure functions)
                IOrder order = ExecuteBusinessLogic(command, existingProducts);

                // 3. Salvăm în baza de date DOAR dacă comanda a fost plasată cu succes
                if (order is PlacedOrder placedOrder)
                {
                    await ordersRepository.SaveOrderAsync(placedOrder);
                }

                // 4. Returnăm evenimentul corespunzător stării
                return order.ToEvent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while placing the order");
                return new OrderPlaceFailedEvent("Unexpected error occurred while processing the order.");
            }
        }

        private static IOrder ExecuteBusinessLogic(
            PlaceOrderCommand command,
            List<ExistingProduct> existingProducts)
        {
            // Funcție pentru verificarea existenței produsului
            Func<ProductCode, bool> checkProductExists = productCode =>
                existingProducts.Any(p => p.ProductCode.Value == productCode.Value);

            // Starea inițială - comanda nevalidată
            UnvalidatedOrder unvalidatedOrder = new(command.InputOrderItems, command.ClientEmail, command.ShippingAddress);

            // Transform chain - exact ca în Lab 8
            IOrder order = new ValidateOrderOperation(checkProductExists).Transform(unvalidatedOrder);
            order = new CalculateOrderOperation().Transform(order, existingProducts);
            order = new PlaceOrderOperation().Transform(order);

            return order;
        }
    }
}
