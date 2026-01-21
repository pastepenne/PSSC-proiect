namespace Orders.Domain.Exceptions
{
    public class InvalidShippingAddressException : Exception
    {
        public InvalidShippingAddressException(string message) : base(message) { }
    }
}
