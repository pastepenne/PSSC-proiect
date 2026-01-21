using static Orders.Domain.Models.Order;

namespace Orders.Domain.Models
{
    public static class OrderPlacedEvent
    {
        public interface IOrderPlacedEvent { }

        public record OrderPlaceSucceededEvent : IOrderPlacedEvent
        {
            public string OrderNumber { get; }
            public DateTime PlacedDate { get; }
            public IEnumerable<CalculatedOrderItem> OrderItems { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
            public decimal TotalPrice { get; }

            internal OrderPlaceSucceededEvent(string orderNumber, IEnumerable<CalculatedOrderItem> orderItems, DateTime placedDate, string clientEmail, string shippingAddress, decimal totalPrice)
            {
                OrderNumber = orderNumber;
                PlacedDate = placedDate;
                OrderItems = orderItems;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalPrice = totalPrice;
            }
        }

        public record OrderPlaceFailedEvent : IOrderPlacedEvent
        {
            public IEnumerable<string> Reasons { get; }

            internal OrderPlaceFailedEvent(string reason)
            {
                Reasons = new[] { reason };
            }

            internal OrderPlaceFailedEvent(IEnumerable<string> reasons)
            {
                Reasons = reasons;
            }
        }

        public static IOrderPlacedEvent ToEvent(this IOrder order) =>
            order switch
            {
                UnvalidatedOrder _ => new OrderPlaceFailedEvent("Unexpected unvalidated state"),
                ValidatedOrder _ => new OrderPlaceFailedEvent("Unexpected validated state"),
                CalculatedOrder _ => new OrderPlaceFailedEvent("Unexpected calculated state"),
                InvalidOrder invalidOrder => new OrderPlaceFailedEvent(invalidOrder.Reasons),
                PlacedOrder placedOrder => new OrderPlaceSucceededEvent(
                    placedOrder.OrderNumber,
                    placedOrder.ItemList,
                    placedOrder.PlacedDate,
                    placedOrder.ClientEmail.Value,
                    placedOrder.ShippingAddress.Value,
                    placedOrder.TotalPrice.Value),
                _ => throw new NotImplementedException()
            };
    }
}
