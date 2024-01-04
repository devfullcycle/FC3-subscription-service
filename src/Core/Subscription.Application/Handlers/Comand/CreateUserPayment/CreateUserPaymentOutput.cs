namespace Subscription.Application.Handlers.Comand.CreateUserPayment
{
    public class CreateUserPaymentOutput
    {
        public int UserSubscriptionId { get; private set; }

        public static CreateUserPaymentOutput Create(int id) => new() { UserSubscriptionId = id };
    }
}
