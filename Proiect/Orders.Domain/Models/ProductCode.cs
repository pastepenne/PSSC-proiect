using System.Text.RegularExpressions;
using Orders.Domain.Exceptions;

namespace Orders.Domain.Models
{
    public record ProductCode
    {
        // Pattern: PRD-XXXXX (ex: PRD-00001)
        public const string Pattern = "^PRD-[0-9]{5}$";
        private static readonly Regex ValidPattern = new(Pattern);

        public string Value { get; }

        public ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException($"{value} is not a valid product code.");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString() => Value;

        public static bool TryParse(string stringValue, out ProductCode? productCode)
        {
            bool isValid = false;
            productCode = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                productCode = new ProductCode(stringValue);
            }

            return isValid;
        }
    }
}
