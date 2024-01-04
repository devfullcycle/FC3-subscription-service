using Subscription.Domain.Entities;

namespace Subscription.Application.Handlers.Query.GetPlans
{
    public class GetPlansOutput
    {
        public List<Plan> Plans { get; set; } = new();

        public static GetPlansOutput Create(List<Plan> plans)
        {
            var data = new GetPlansOutput();
            data.Plans.AddRange(plans);
            return data;
        }
    }
}
