namespace Orders.Domain.Operations
{
    internal abstract class DomainOperation<TInput, TState, TOutput>
        where TState : class
    {
        public abstract TOutput Transform(TInput input, TState? state);
    }
}
