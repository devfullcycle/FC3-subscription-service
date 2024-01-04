using FluentValidation;

namespace Subscription.Application.Handlers.Comand.CreateUser
{
    public class CreateuserInputValidator : AbstractValidator<CreateUserInput>
    {
        public CreateuserInputValidator()
        {
            RuleFor(item => item.Name).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(item => item.LastName).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(item => item.Street).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(item => item.City).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(item => item.ZipCode).NotNull().NotEmpty().MinimumLength(8);
            RuleFor(item => item.State).NotNull().NotEmpty().Length(2);
            RuleFor(item => item.Country).NotNull().NotEmpty().Length(2);
            RuleFor(item => item.DocumentNumber).NotNull().NotEmpty().MinimumLength(11).MaximumLength(14);
            RuleFor(item => item.Age).NotNull().NotEmpty().GreaterThan(17);
        }
    }
}
