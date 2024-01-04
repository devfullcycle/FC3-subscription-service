using Subscription.Application.Abstractions.Handlers;
using Subscription.Domain.Enum;

namespace Subscription.Application.Handlers.Comand.CreateUserPayment
{
    public class CreateUserPaymentInput : IInput
    {
        public CreateUserPaymentInput(Guid planId, 
            PlanType planType, 
            decimal planCost, 
            Guid userId,
            string email,
            PaymentType paymentType, Guid correlationId)
        {
            PlanId = planId;
            Email = email;
            PlanType = planType;
            PlanCost = planCost;
            UserId = userId;
            PaymentType = paymentType;
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }

        public void CreateCorrelation(Guid? correlationId = null)
        {
            CorrelationId = correlationId is not null ? correlationId.Value : Guid.NewGuid();
        }

        public Guid PlanId { get; set; }
        public PlanType PlanType { get; set; }
        public decimal PlanCost { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}
