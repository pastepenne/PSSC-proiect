using Orders.Domain.Exceptions;

namespace Orders.Domain.Models
{
    public record ShippingAddress
    {
        public string Value { get; }

        internal ShippingAddress(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidShippingAddressException($"Shipping address is not valid.");
            }
        }

        private static bool IsValid(string stringValue) => !string.IsNullOrWhiteSpace(stringValue) && stringValue.Length >= 10 && stringValue.Length <= 200;

        public override string ToString() => Value;

        public static bool TryParse(string stringValue, out ShippingAddress? shippingAddress)
        {
            bool isValid = false;
            shippingAddress = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                shippingAddress = new ShippingAddress(stringValue);
            }

            return isValid;
        }
    }
}
