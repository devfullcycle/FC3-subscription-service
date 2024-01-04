using FluentValidation;

namespace Subscription.Application.Handlers.Query.GetSusbscribers
{
    public class GetSubscribersInputValidator : AbstractValidator<GetSubscribersInput>
    {
        public GetSubscribersInputValidator()
        {
            RuleFor(item => item.Page).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(item => item.Size).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
