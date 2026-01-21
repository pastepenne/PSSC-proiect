using Orders.Domain.Models;
using static Orders.Domain.Models.Order;

namespace Orders.Domain.Operations
{
    // ExistingProduct reprezintă produsul din baza de date cu prețul lui
    public record ExistingProduct(ProductCode ProductCode, Price UnitPrice);

    internal sealed class CalculateOrderOperation : OrderOperation<List<ExistingProduct>>
    {
        protected override IOrder OnValid(ValidatedOrder validOrder, List<ExistingProduct>? existingProducts)
        {
            if (existingProducts is null)
            {
                return validOrder;
            }

            var productPrices = existingProducts.ToDictionary(p => p.ProductCode.Value, p => p.UnitPrice);

            List<CalculatedOrderItem> calculatedItems = validOrder.ItemList
                .Select(item =>
                {
                    var unitPrice = productPrices[item.ProductCode.Value];
                    var totalPrice = unitPrice * item.Quantity.Value;
                    return new CalculatedOrderItem(item.ProductCode, item.Quantity, unitPrice, totalPrice);
                })
                .ToList();

            var totalOrderPrice = calculatedItems
                .Aggregate(Price.Zero, (sum, item) => sum + item.TotalPrice);

            return new CalculatedOrder(calculatedItems, validOrder.ClientEmail, validOrder.ShippingAddress, totalOrderPrice);
        }
    }
}
