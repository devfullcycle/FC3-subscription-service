using FluentValidation;

namespace Subscription.Application.Handlers.Comand.CreatePlanCost
{
    public class CreatePlanCostInputValidator : AbstractValidator<CreatePlanCostInput>
    {
        public CreatePlanCostInputValidator()
        {
            RuleFor(item => item.Price).GreaterThan(0).NotNull().NotEmpty();
            RuleFor(item => item.PlanId).NotEmpty().NotNull().NotEqual(Guid.Empty);
        }
    }
}
