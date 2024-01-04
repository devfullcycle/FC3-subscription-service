using MediatR;

namespace Subscription.Application.Abstractions.Handlers
{
    public interface IInput : IRequest
    {
        Guid CorrelationId { get; }
        public void CreateCorrelation(Guid? correlationId = null);
    }
}
