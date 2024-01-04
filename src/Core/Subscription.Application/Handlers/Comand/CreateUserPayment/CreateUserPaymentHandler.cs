using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.AuthService;
using Subscription.Application.Adapters.PaymentGateway;
using Subscription.Application.Adapters.Repository;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;

namespace Subscription.Application.Handlers.Comand.CreateUserPayment
{
    public class CreateUserPaymentHandler :
    BaseHandler<CreateUserPaymentInput, CreateUserPaymentOutput>,
        ICreateUserPaymentHandler
    {
        private readonly ISubscriptionRepository _repository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IAuthorizationService _authorization;

        public CreateUserPaymentHandler(ILogger<CreateUserPaymentInput> logger,
            ISubscriptionRepository repository,
            IPaymentGateway paymentGateway,
            IAuthorizationService authorization) : 
            base(logger, new CreateUserPaymentInputValidator())
        {
            _repository = repository;
            _paymentGateway = paymentGateway;
            _authorization = authorization;
        }
        public override async Task Handle(CreateUserPaymentInput input, CancellationToken cancellationToken)
        {
            LogInformation("Create User Payment Starting");
            User user = await _repository.GetUserByIdAsync(input.UserId, cancellationToken);
            Plan plan = await _repository.GetPlanByIdAsync(input.PlanId, input.PlanType,  cancellationToken);

            if (!user.IsValid || !plan.IsValid)
            {
                Response.AddMessages(user.Notifications);
                Response.AddMessages(plan.Notifications);
                return;
            }

            var planCost = plan.PlanCosts.FirstOrDefault(item => item.Price.Equals(input.PlanCost));

            if (planCost is null)
            {
                Response.AddMessage("Plan cost not found");
                return;
            }
            
            var subscriptionValidation = await _repository.CheckIfUserHasActiveSubscription(user.Id, plan.Id, cancellationToken);

            if (subscriptionValidation)
            {
                Response.AddMessage("User already has subscription");
                return;
            }

            var payment = input.PaymentType.Equals(PaymentType.Pix) ?
                               PaymentInput.CreatePix(user.Name, user.LastName, user.Document.DocumentNumber, input.PlanCost) :
                               PaymentInput.CreateCreditCard(user.Name, user.LastName, user.Document.DocumentNumber, input.PlanCost);

            LogInformation("Sending payment to gateway");

            var response = await _paymentGateway.MakePaymentAsync(payment, cancellationToken);

            if (!response)
            {
                Response.AddMessage("Payment not accepted");
                return;
            }

            var data = await _repository.CreateSubscriptionOnDatabaseasync(UserSubscription.Create(user, plan, DateTime.Now, true, false), cancellationToken);

            if (!data.IsValid)
                Response.AddMessages(data.Notifications);

            await _authorization.AddSubscriberRoleToUser(input.Email, cancellationToken);

            LogInformation("Create User Payment Ended");
        }
    }
}
