using Orders.Dto.Models;
using System.Text;

namespace Orders.Dto.Events
{
    // Eventul trimis catre Invoicing cand o comanda a fost plasata cu succes
    public record OrderPlacedEvent
    {
        public string OrderNumber { get; init; } = string.Empty;
        public DateTime PlacedDate { get; init; }
        public string ClientEmail { get; init; } = string.Empty;
        public string ShippingAddress { get; init; } = string.Empty;
        public decimal TotalPrice { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine("===== ORDER PLACED EVENT =====");
            sb.AppendLine($"Order Number: {OrderNumber}");
            sb.AppendLine($"Placed Date: {PlacedDate}");
            sb.AppendLine($"Client Email: {ClientEmail}");
            sb.AppendLine($"Shipping Address: {ShippingAddress}");
            sb.AppendLine($"Total Price: {TotalPrice:F2} RON");
            sb.AppendLine("Items:");
            foreach (var item in Items)
            {
                sb.AppendLine($"  - {item.ProductCode}: {item.Quantity} x {item.UnitPrice:F2} = {item.TotalPrice:F2} RON");
            }
            sb.AppendLine("==============================");
            return sb.ToString();
        }
    }
}
