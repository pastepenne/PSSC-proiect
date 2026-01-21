using static Shipping.Domain.Models.Shipment;

namespace Shipping.Domain.Operations
{
    internal abstract class ShipmentOperation
    {
        internal IShipment Transform(IShipment shipment) => shipment switch
        {
            UnprocessedShipment unprocessed => OnUnprocessed(unprocessed),
            PreparedShipment prepared => OnPrepared(prepared),
            ShippedShipment shipped => OnShipped(shipped),
            FailedShipment failed => OnFailed(failed),
            _ => throw new InvalidOperationException($"Invalid shipment state: {shipment.GetType().Name}")
        };

        protected virtual IShipment OnUnprocessed(UnprocessedShipment unprocessed) => unprocessed;
        protected virtual IShipment OnPrepared(PreparedShipment prepared) => prepared;
        protected virtual IShipment OnShipped(ShippedShipment shipped) => shipped;
        protected virtual IShipment OnFailed(FailedShipment failed) => failed;
    }
}
