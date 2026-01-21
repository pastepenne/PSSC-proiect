using Orders.Domain.Exceptions;

namespace Orders.Domain.Models
{
    public record Quantity
    {
        public int Value { get; }

        internal Quantity(int value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidQuantityException($"{value} is not a valid quantity.");
            }
        }

        private static bool IsValid(int value) => value > 0 && value <= 100;

        public override string ToString() => Value.ToString();

        public static bool TryParse(int value, out Quantity? quantity)
        {
            bool isValid = false;
            quantity = null;

            if (IsValid(value))
            {
                isValid = true;
                quantity = new Quantity(value);
            }

            return isValid;
        }
    }
}
