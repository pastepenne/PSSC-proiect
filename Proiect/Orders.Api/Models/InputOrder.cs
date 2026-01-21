using System.ComponentModel.DataAnnotations;

namespace Orders.Api.Models
{
    public class InputOrder
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(10, ErrorMessage = "Shipping address must be at least 10 characters")]
        [MaxLength(200, ErrorMessage = "Shipping address cannot exceed 200 characters")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<InputOrderItem> Items { get; set; } = new();
    }
}