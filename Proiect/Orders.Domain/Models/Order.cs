namespace Orders.Domain.Models
{
    public static class Order
    {
        public interface IOrder { }

        // Starea initiala - comanda nevalidata
        public record UnvalidatedOrder : IOrder
        {
            public UnvalidatedOrder(IReadOnlyCollection<UnvalidatedOrderItem> itemList, string clientEmail, string shippingAddress)
            {
                ItemList = itemList;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
            }

            public IReadOnlyCollection<UnvalidatedOrderItem> ItemList { get; }
            public string ClientEmail { get; }
            public string ShippingAddress { get; }
        }

        // Starea de eroare - comanda invalida
        public record InvalidOrder : IOrder
        {
            internal InvalidOrder(IReadOnlyCollection<UnvalidatedOrderItem> itemList, IEnumerable<string> reasons)
            {
                ItemList = itemList;
                Reasons = reasons;
            }

            public IReadOnlyCollection<UnvalidatedOrderItem> ItemList { get; }
            public IEnumerable<string> Reasons { get; }
        }

        // Starea validata
        public record ValidatedOrder : IOrder
        {
            internal ValidatedOrder(IReadOnlyCollection<ValidatedOrderItem> itemList, ClientEmail clientEmail, ShippingAddress shippingAddress)
            {
                ItemList = itemList;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
            }

            public IReadOnlyCollection<ValidatedOrderItem> ItemList { get; }
            public ClientEmail ClientEmail { get; }
            public ShippingAddress ShippingAddress { get; }
        }

        // Starea calculata (cu total calculat)
        public record CalculatedOrder : IOrder
        {
            internal CalculatedOrder(IReadOnlyCollection<CalculatedOrderItem> itemList, ClientEmail clientEmail, ShippingAddress shippingAddress, Price totalPrice)
            {
                ItemList = itemList;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalPrice = totalPrice;
            }

            public IReadOnlyCollection<CalculatedOrderItem> ItemList { get; }
            public ClientEmail ClientEmail { get; }
            public ShippingAddress ShippingAddress { get; }
            public Price TotalPrice { get; }
        }

        // Starea finala - comanda plasata
        public record PlacedOrder : IOrder
        {
            internal PlacedOrder(IReadOnlyCollection<CalculatedOrderItem> itemList, ClientEmail clientEmail, ShippingAddress shippingAddress, Price totalPrice, DateTime placedDate, string orderNumber)
            {
                ItemList = itemList;
                ClientEmail = clientEmail;
                ShippingAddress = shippingAddress;
                TotalPrice = totalPrice;
                PlacedDate = placedDate;
                OrderNumber = orderNumber;
            }

            public IReadOnlyCollection<CalculatedOrderItem> ItemList { get; }
            public ClientEmail ClientEmail { get; }
            public ShippingAddress ShippingAddress { get; }
            public Price TotalPrice { get; }
            public DateTime PlacedDate { get; }
            public string OrderNumber { get; }
        }
    }
}
