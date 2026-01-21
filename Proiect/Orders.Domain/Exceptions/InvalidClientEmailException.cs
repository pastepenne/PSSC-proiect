namespace Orders.Domain.Exceptions
{
    public class InvalidClientEmailException : Exception
    {
        public InvalidClientEmailException(string message) : base(message) { }
    }
}
