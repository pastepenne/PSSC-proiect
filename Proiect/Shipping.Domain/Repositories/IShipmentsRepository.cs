using static Shipping.Domain.Models.Shipment;

namespace Shipping.Domain.Repositories
{
    public interface IShipmentsRepository
    {
        Task SaveShipmentAsync(ShippedShipment shipment);
    }
}
