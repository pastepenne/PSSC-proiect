using Orders.Domain.Exceptions;

namespace Orders.Domain.Models
{
    public record Price
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidPriceException($"{value} is not a valid price.");
            }
        }

        private static bool IsValid(decimal value) => value >= 0;

        public static Price operator +(Price a, Price b) => new(a.Value + b.Value);
        
        public static Price operator *(Price price, int quantity) => new(price.Value * quantity);

        public override string ToString() => $"{Value:F2}";

        public static bool TryParse(decimal value, out Price? price)
        {
            bool isValid = false;
            price = null;

            if (IsValid(value))
            {
                isValid = true;
                price = new Price(value);
            }

            return isValid;
        }

        public static Price Zero => new(0);
    }
}
