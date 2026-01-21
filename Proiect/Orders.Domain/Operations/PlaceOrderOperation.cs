using static Orders.Domain.Models.Order;

namespace Orders.Domain.Operations
{
    internal sealed class PlaceOrderOperation : OrderOperation
    {
        protected override IOrder OnCalculated(CalculatedOrder calculatedOrder)
        {
            // Generam un numar unic de comanda
            string orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            return new PlacedOrder(
                calculatedOrder.ItemList,
                calculatedOrder.ClientEmail,
                calculatedOrder.ShippingAddress,
                calculatedOrder.TotalPrice,
                DateTime.UtcNow,
                orderNumber);
        }
    }
}
