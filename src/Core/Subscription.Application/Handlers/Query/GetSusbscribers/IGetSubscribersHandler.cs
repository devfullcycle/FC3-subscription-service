using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetSusbscribers
{
    public interface IGetSubscribersHandler : IBaseHandler<GetSubscribersInput, GetSubscribersOutput>
    {
    }
}
