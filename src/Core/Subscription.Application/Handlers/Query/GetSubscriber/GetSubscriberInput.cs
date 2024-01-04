using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetSubscriber
{
    public class GetSubscriberInput : Input
    {
        public GetSubscriberInput(Guid subscriberId, Guid? guid) : base(guid)
        {
            SubscriberId = subscriberId;
        }
        public Guid SubscriberId { get; set; }
    }
}
