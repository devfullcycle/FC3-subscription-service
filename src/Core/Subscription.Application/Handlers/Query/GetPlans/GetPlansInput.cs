using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Query.GetPlans
{
    public class GetPlansInput : Input
    {
        public GetPlansInput(int page, int size, Guid? correlationId) : base(correlationId)
        {
            Page = page;
            Size = size;
        }

        public int Page { get; set; }
        public int Size { get; set; }
    }
}
