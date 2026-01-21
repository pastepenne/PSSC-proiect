namespace Orders.Domain.Exceptions
{
    public class InvalidProductCodeException : Exception
    {
        public InvalidProductCodeException(string message) : base(message) { }
    }
}
