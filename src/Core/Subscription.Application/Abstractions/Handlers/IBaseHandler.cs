using MediatR;

namespace Subscription.Application.Abstractions.Handlers
{
    public interface IBaseHandler<TInput, TOutput> : IRequestHandler<TInput>
        where TInput : IInput
        where TOutput : class
    {
        Task<Output<TOutput>> HandleExecutionAsync(TInput input, CancellationToken cancellationToken);
    }
}
