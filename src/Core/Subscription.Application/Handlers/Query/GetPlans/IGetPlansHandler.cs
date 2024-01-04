using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetPlans
{
    public interface IGetPlansHandler : IBaseHandler<GetPlansInput, GetPlansOutput>
    {
    }
}
