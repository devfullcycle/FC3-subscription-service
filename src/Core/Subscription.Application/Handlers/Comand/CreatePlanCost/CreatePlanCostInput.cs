using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Comand.CreatePlanCost
{
    public class CreatePlanCostInput : IInput
    {
        public CreatePlanCostInput(Guid planId, decimal price, Guid correlationId)
        {
            PlanId = planId;
            Price = price;
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }

        public void CreateCorrelation(Guid? correlationId = null)
        {
            CorrelationId = correlationId is not null ? correlationId.Value : Guid.NewGuid();
        }

        public Guid PlanId { get; set; }
        public decimal Price { get; set; }
    }
}
