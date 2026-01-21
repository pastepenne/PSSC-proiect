using Orders.Domain.Models;
using static Orders.Domain.Models.Order;

namespace Orders.Domain.Operations
{
    internal sealed class ValidateOrderOperation : OrderOperation
    {
        private readonly Func<ProductCode, bool> checkProductExists;

        internal ValidateOrderOperation(Func<ProductCode, bool> checkProductExists)
        {
            this.checkProductExists = checkProductExists;
        }

        protected override IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder)
        {
            (List<ValidatedOrderItem> validatedItems, List<string> validationErrors) = ValidateListOfItems(unvalidatedOrder);
            
            // Validam si email-ul si adresa de shipping
            ClientEmail? clientEmail = ValidateClientEmail(unvalidatedOrder.ClientEmail, validationErrors);
            ShippingAddress? shippingAddress = ValidateShippingAddress(unvalidatedOrder.ShippingAddress, validationErrors);

            if (validationErrors.Any())
            {
                return new InvalidOrder(unvalidatedOrder.ItemList, validationErrors);
            }
            else
            {
                return new ValidatedOrder(validatedItems, clientEmail!, shippingAddress!);
            }
        }

        private (List<ValidatedOrderItem>, List<string>) ValidateListOfItems(UnvalidatedOrder order)
        {
            List<string> validationErrors = new();
            List<ValidatedOrderItem> validatedItems = new();

            foreach (var unvalidatedItem in order.ItemList)
            {
                ValidatedOrderItem? validItem = ValidateItem(unvalidatedItem, validationErrors);
                if (validItem is not null)
                {
                    validatedItems.Add(validItem);
                }
            }

            return (validatedItems, validationErrors);
        }

        private ValidatedOrderItem? ValidateItem(UnvalidatedOrderItem unvalidatedItem, List<string> validationErrors)
        {
            List<string> currentErrors = new();

            ProductCode? productCode = ValidateProductCode(unvalidatedItem, currentErrors);
            Quantity? quantity = ValidateQuantity(unvalidatedItem, currentErrors);

            ValidatedOrderItem? validItem = null;
            if (!currentErrors.Any())
            {
                validItem = new ValidatedOrderItem(productCode!, quantity!);
            }
            else
            {
                validationErrors.AddRange(currentErrors);
            }

            return validItem;
        }

        private ProductCode? ValidateProductCode(UnvalidatedOrderItem item, List<string> errors)
        {
            // Validare sintactica
            if (!ProductCode.TryParse(item.ProductCode, out ProductCode? productCode))
            {
                errors.Add($"Invalid product code format: {item.ProductCode}");
                return null;
            }

            // Validare semantica - produsul exista?
            if (!checkProductExists(productCode!))
            {
                errors.Add($"Product not found: {item.ProductCode}");
                return null;
            }

            return productCode;
        }

        private static Quantity? ValidateQuantity(UnvalidatedOrderItem item, List<string> errors)
        {
            if (!Quantity.TryParse(item.Quantity, out Quantity? quantity))
            {
                errors.Add($"Invalid quantity for product {item.ProductCode}: {item.Quantity}. Must be between 1 and 100.");
                return null;
            }

            return quantity;
        }

        private static ClientEmail? ValidateClientEmail(string email, List<string> errors)
        {
            if (!ClientEmail.TryParse(email, out ClientEmail? clientEmail))
            {
                errors.Add($"Invalid client email: {email}");
                return null;
            }

            return clientEmail;
        }

        private static ShippingAddress? ValidateShippingAddress(string address, List<string> errors)
        {
            if (!ShippingAddress.TryParse(address, out ShippingAddress? shippingAddress))
            {
                errors.Add($"Invalid shipping address. Must be between 10 and 200 characters.");
                return null;
            }

            return shippingAddress;
        }
    }
}
