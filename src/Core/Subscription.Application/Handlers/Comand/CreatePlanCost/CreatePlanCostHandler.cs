using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Domain.Entities;

namespace Subscription.Application.Handlers.Comand.CreatePlanCost
{
    public class CreatePlanCostHandler :
        BaseHandler<CreatePlanCostInput, CreatePlanCostOutput>,
        ICreatePlanCostHandler
    {
        private readonly ISubscriptionRepository _repository;

        public CreatePlanCostHandler(ILogger<CreatePlanCostInput> logger, 
            ISubscriptionRepository repository) : 
            base(logger, new CreatePlanCostInputValidator())
        {
            _repository = repository;
        }

        public async override Task Handle(CreatePlanCostInput input, CancellationToken cancellationToken)
        {
            LogInformation("Savind plan cost on database");

            var planCost = PlanCost.Create(input.Price, input.PlanId);

            if (!planCost.IsValid)
            {
                Response.AddMessages(planCost.Notifications);
                return;
            }

            var plan = await _repository.GetPlanByIdAsync(input.PlanId, cancellationToken);

            if(plan is null || plan.Id.Equals(Guid.Empty))
            {
                Response.AddMessage("Plan not found");
                return;
            }

            var response = await _repository.CreatePlanCostAsync(planCost, cancellationToken);

            Response.AddResult(CreatePlanCostOutput.Create(response.Id));
            LogInformation("Saved plan cost on database");
        }
    }
}
