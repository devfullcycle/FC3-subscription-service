using Subscription.Application.Handlers.Comand.CreatePlan;

namespace Subscription.Application.Handlers.Comand.CreatePlanCost
{
    public class CreatePlanCostOutput
    {
        public Guid Id { get; private set; }
        public static CreatePlanCostOutput Create(Guid id)
        {
            CreatePlanCostOutput planOutput = new()
            {
                Id = id
            };
            return planOutput;
        }
    }
}
