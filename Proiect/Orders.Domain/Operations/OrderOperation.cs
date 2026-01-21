using Orders.Domain.Exceptions;
using static Orders.Domain.Models.Order;

namespace Orders.Domain.Operations
{
    internal abstract class OrderOperation<TState> : DomainOperation<IOrder, TState, IOrder>
        where TState : class
    {
        public override IOrder Transform(IOrder order, TState? state) => order switch
        {
            UnvalidatedOrder unvalidatedOrder => OnUnvalidated(unvalidatedOrder, state),
            ValidatedOrder validOrder => OnValid(validOrder, state),
            InvalidOrder invalidOrder => OnInvalid(invalidOrder, state),
            CalculatedOrder calculatedOrder => OnCalculated(calculatedOrder, state),
            PlacedOrder placedOrder => OnPlaced(placedOrder, state),
            _ => throw new InvalidOrderStateException(order.GetType().Name)
        };

        protected virtual IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder, TState? state) => unvalidatedOrder;

        protected virtual IOrder OnValid(ValidatedOrder validOrder, TState? state) => validOrder;

        protected virtual IOrder OnPlaced(PlacedOrder placedOrder, TState? state) => placedOrder;

        protected virtual IOrder OnCalculated(CalculatedOrder calculatedOrder, TState? state) => calculatedOrder;

        protected virtual IOrder OnInvalid(InvalidOrder invalidOrder, TState? state) => invalidOrder;
    }

    internal abstract class OrderOperation : OrderOperation<object>
    {
        internal IOrder Transform(IOrder order) => Transform(order, null);

        protected sealed override IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder, object? state) => OnUnvalidated(unvalidatedOrder);

        protected virtual IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder) => unvalidatedOrder;

        protected sealed override IOrder OnValid(ValidatedOrder validOrder, object? state) => OnValid(validOrder);

        protected virtual IOrder OnValid(ValidatedOrder validOrder) => validOrder;

        protected sealed override IOrder OnPlaced(PlacedOrder placedOrder, object? state) => OnPlaced(placedOrder);

        protected virtual IOrder OnPlaced(PlacedOrder placedOrder) => placedOrder;

        protected sealed override IOrder OnCalculated(CalculatedOrder calculatedOrder, object? state) => OnCalculated(calculatedOrder);

        protected virtual IOrder OnCalculated(CalculatedOrder calculatedOrder) => calculatedOrder;

        protected sealed override IOrder OnInvalid(InvalidOrder invalidOrder, object? state) => OnInvalid(invalidOrder);

        protected virtual IOrder OnInvalid(InvalidOrder invalidOrder) => invalidOrder;
    }
}
