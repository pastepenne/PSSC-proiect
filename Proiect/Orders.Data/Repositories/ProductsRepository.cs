using Microsoft.EntityFrameworkCore;
using Orders.Domain.Models;
using Orders.Domain.Operations;
using Orders.Domain.Repositories;

namespace Orders.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly OrdersContext dbContext;

        public ProductsRepository(OrdersContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ExistingProduct>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck)
        {
            var products = await dbContext.Products
                .Where(p => productCodesToCheck.Contains(p.Code))
                .AsNoTracking()
                .ToListAsync();

            return products.Select(p => new ExistingProduct(
                new ProductCode(p.Code),
                new Price(p.Price)
            )).ToList();
        }
    }
}
