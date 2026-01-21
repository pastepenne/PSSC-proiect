namespace Orders.Data.Models
{
    // DTO pentru tabelul Product (prepopulat)
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
