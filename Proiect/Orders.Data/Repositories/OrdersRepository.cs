using Orders.Data.Models;
using Orders.Domain.Repositories;
using static Orders.Domain.Models.Order;

namespace Orders.Data.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersContext dbContext;

        public OrdersRepository(OrdersContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveOrderAsync(PlacedOrder order)
        {
            var orderDto = new OrderDto
            {
                OrderNumber = order.OrderNumber,
                ClientEmail = order.ClientEmail.Value,
                ShippingAddress = order.ShippingAddress.Value,
                TotalPrice = order.TotalPrice.Value,
                PlacedDate = order.PlacedDate,
                Items = order.ItemList.Select(item => new OrderItemDto
                {
                    ProductCode = item.ProductCode.Value,
                    Quantity = item.Quantity.Value,
                    UnitPrice = item.UnitPrice.Value,
                    TotalPrice = item.TotalPrice.Value
                }).ToList()
            };

            dbContext.Orders.Add(orderDto);
            await dbContext.SaveChangesAsync();
        }
    }
}
