using FluentValidation;

namespace Subscription.Application.Handlers.Query.GetSubscriber
{
    public class GetSubscriberInputValidator : AbstractValidator<GetSubscriberInput>
    {
        public GetSubscriberInputValidator()
        {
            RuleFor(item => item.SubscriberId).NotNull().NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
