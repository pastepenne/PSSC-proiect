namespace Orders.Domain.Models
{
    public record PlaceOrderCommand
    {
        public PlaceOrderCommand(IReadOnlyCollection<UnvalidatedOrderItem> inputOrderItems, string clientEmail, string shippingAddress)
        {
            InputOrderItems = inputOrderItems;
            ClientEmail = clientEmail;
            ShippingAddress = shippingAddress;
        }

        public IReadOnlyCollection<UnvalidatedOrderItem> InputOrderItems { get; }
        public string ClientEmail { get; }
        public string ShippingAddress { get; }
    }
}
