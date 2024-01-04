using FluentValidation;

namespace Subscription.Application.Handlers.Comand.CreatePlan
{
    public class CreatePlanInputValidator : AbstractValidator<CreatePlanInput>
    {
        public CreatePlanInputValidator()
        {
            RuleFor(item => item.Name).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(item => item.Description).NotEmpty().NotNull().MinimumLength(10);
        }
    }
}
