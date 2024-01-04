namespace Subscription.Application.Handlers.Comand.CreatePlan
{
    public class CreatePlanOutput
    {
        public Guid Id { get; private set; }
        public static CreatePlanOutput Create(Guid id)
        {
            CreatePlanOutput planOutput = new() {
                Id = id
            };
            return planOutput;
        }
    }
}
