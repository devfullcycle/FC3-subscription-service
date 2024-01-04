using FluentValidation;

namespace Subscription.Application.Handlers.Query.GetPlans
{
    public class GetPlansInputValidator : AbstractValidator<GetPlansInput>
    {
        public GetPlansInputValidator()
        {
            RuleFor(item => item.Page).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(item => item.Size).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
