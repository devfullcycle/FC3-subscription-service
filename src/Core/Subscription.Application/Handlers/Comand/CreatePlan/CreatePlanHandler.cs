using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;

namespace Subscription.Application.Handlers.Comand.CreatePlan
{
    public class CreatePlanHandler :
        BaseHandler<CreatePlanInput, CreatePlanOutput>,
        ICreatePlanHandler
    {
        private readonly ISubscriptionRepository _repository;

        public CreatePlanHandler(ILogger<CreatePlanInput> logger, ISubscriptionRepository repository) :
            base(logger, new CreatePlanInputValidator())
        {
            _repository = repository;
        }
        public async override Task Handle(CreatePlanInput input, CancellationToken cancellationToken)
        {
            LogInformation("Savin plan on database");
            Plan plan;

            if (input.PlanType.Equals(PlanType.MONTHLY))
                plan = Plan.CreateMonthly(input.Name, input.Description);
            else
                plan = Plan.CreateAnnually(input.Name, input.Description);

            if (!plan.IsValid){
                Response.AddMessages(plan.Notifications);
                return;
            }

            var response = await _repository.CreatePlanAsync(plan, cancellationToken);

            if (!response.IsValid)
            {
                Response.AddMessages(response.Notifications);
                return;
            }

            Response.AddResult(CreatePlanOutput.Create(response.Id));

            LogInformation("Saved plan on database");
        }
    }
}
