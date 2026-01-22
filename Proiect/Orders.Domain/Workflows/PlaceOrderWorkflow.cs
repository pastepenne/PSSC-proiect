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

        // Injectam dependintele necesare
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
                // Citim din baza de date produsele pentru validare semantica si preturi
                IEnumerable<string> productCodesToCheck = command.InputOrderItems.Select(item => item.ProductCode);
                List<ExistingProduct> existingProducts = await productsRepository.GetExistingProductsAsync(productCodesToCheck);

                // Executam logica de business (pure functions)
                IOrder order = ExecuteBusinessLogic(command, existingProducts);

                // Salvam in baza de date doar daca comanda a fost plasata cu succes
                if (order is PlacedOrder placedOrder)
                {
                    await ordersRepository.SaveOrderAsync(placedOrder);

                    foreach (var orderItem in command.InputOrderItems)
                    {
                        await productsRepository.UpdateStockAsync(orderItem.ProductCode, orderItem.Quantity);
                    }
                }

                // Returnam evenimentul corespunzator starii
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
            // Functie pentru verificarea disponibilitatii produsului (cod produs existent si cantitate disponibila)
            Func<(ProductCode, Quantity), bool> checkProductAvailable = (productCodeAndQuantity) =>
            {
                ProductCode productCode = productCodeAndQuantity.Item1;
                Quantity quantity = productCodeAndQuantity.Item2;
                
                ExistingProduct? product = existingProducts.FirstOrDefault(p => p.ProductCode.Value == productCode.Value);
                return product is not null && product.Available.Value >= quantity.Value;
            };

            // Starea initiala - comanda nevalidata
            UnvalidatedOrder unvalidatedOrder = new(command.InputOrderItems, command.ClientEmail, command.ShippingAddress);

            // Transformam comanda din starea nevalidata pana la plasarea comenzii
            IOrder order = new ValidateOrderOperation(checkProductAvailable).Transform(unvalidatedOrder);
            order = new CalculateOrderOperation().Transform(order, existingProducts);
            order = new PlaceOrderOperation().Transform(order);

            return order;
        }
    }
}
