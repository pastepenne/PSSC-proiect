using Shipping.Data.Models;
using Shipping.Domain.Repositories;
using static Shipping.Domain.Models.Shipment;

namespace Shipping.Data.Repositories
{
    public class ShipmentsRepository : IShipmentsRepository
    {
        private readonly ShippingContext dbContext;

        public ShipmentsRepository(ShippingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveShipmentAsync(ShippedShipment shipment)
        {
            var shipmentDto = new ShipmentDto
            {
                TrackingNumber = shipment.TrackingNumber,
                OrderNumber = shipment.OrderNumber,
                InvoiceNumber = shipment.InvoiceNumber,
                PreparedDate = shipment.PreparedDate,
                ShippedDate = shipment.ShippedDate,
                CourierName = shipment.CourierName,
                ClientEmail = shipment.ClientEmail,
                ShippingAddress = shipment.ShippingAddress,
                TotalAmount = shipment.TotalAmount,
                Items = shipment.Items.Select(item => new ShipmentItemDto
                {
                    ProductCode = item.ProductCode,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            dbContext.Shipments.Add(shipmentDto);
            await dbContext.SaveChangesAsync();
        }
    }
}
