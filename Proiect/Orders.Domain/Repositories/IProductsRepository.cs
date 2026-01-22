using Orders.Domain.Models;
using Orders.Domain.Operations;

namespace Orders.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task<List<ExistingProduct>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck);
        Task UpdateStockAsync(string productCode, int quantityToDeduct);
    }
}
