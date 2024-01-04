using FluentValidation;

namespace Subscription.Application.Handlers.Comand.CreateUserPayment
{
    public class CreateUserPaymentInputValidator : AbstractValidator<CreateUserPaymentInput>
    {
        public CreateUserPaymentInputValidator()
        {
            RuleFor(item => item.PlanCost).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(item => item.UserId).NotNull().NotEmpty().NotEqual(Guid.Empty);
            RuleFor(item => item.PlanId).NotNull().NotEmpty().NotEqual(Guid.Empty);
            RuleFor(item => item.PaymentType).IsInEnum();
        }
    }
}
