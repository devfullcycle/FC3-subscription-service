using Subscription.Application.Abstractions.Handlers;
using Subscription.Domain.Enum;

namespace Subscription.Application.Handlers.Comand.CreatePlan
{
    public class CreatePlanInput : IInput
    {
        public CreatePlanInput(string name, string description, PlanType planType, Guid correlationId)
        {
            Name = name;
            Description = description;
            PlanType = planType;
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }

        public void CreateCorrelation(Guid? correlationId = null)
        {
            CorrelationId = correlationId is not null ? correlationId.Value : Guid.NewGuid();
        }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public PlanType PlanType { get; set; }
    }
}
