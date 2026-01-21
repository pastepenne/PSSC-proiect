using System.ComponentModel.DataAnnotations;

namespace Orders.Api.Models
{
    public class InputOrderItem
    {
        [Required]
        [RegularExpression(Orders.Domain.Models.ProductCode.Pattern, ErrorMessage = "Product code must be in format PRD-XXXXX")]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}
