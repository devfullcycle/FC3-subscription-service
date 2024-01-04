using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetSubscriber
{
    public interface IGetSubscriberHandler : IBaseHandler<GetSubscriberInput, GetSubscriberOutput>
    {
    }
}
