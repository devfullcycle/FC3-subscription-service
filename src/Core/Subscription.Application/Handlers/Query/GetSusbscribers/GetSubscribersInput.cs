using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetSusbscribers
{
    public class GetSubscribersInput : Input
    {
        public GetSubscribersInput(int page, int size,Guid? CorrelationId)  : 
            base(CorrelationId)
        {
            Page = page;
            Size = size;
        }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
