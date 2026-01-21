namespace Orders.Domain.Exceptions
{
    public class InvalidOrderStateException : Exception
    {
        public InvalidOrderStateException(string message) : base($"Invalid order state: {message}") { }
    }
}
