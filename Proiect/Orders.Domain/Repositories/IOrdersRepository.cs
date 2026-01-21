using static Orders.Domain.Models.Order;

namespace Orders.Domain.Repositories
{
    public interface IOrdersRepository
    {
        Task SaveOrderAsync(PlacedOrder order);
    }
}
