using System.Text.RegularExpressions;
using Orders.Domain.Exceptions;

namespace Orders.Domain.Models
{
    public record ClientEmail
    {
        // Pattern simplu pentru email
        public const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private static readonly Regex ValidPattern = new(Pattern);

        public string Value { get; }

        internal ClientEmail(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidClientEmailException($"{value} is not a valid email address.");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString() => Value;

        public static bool TryParse(string stringValue, out ClientEmail? clientEmail)
        {
            bool isValid = false;
            clientEmail = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                clientEmail = new ClientEmail(stringValue);
            }

            return isValid;
        }
    }
}
