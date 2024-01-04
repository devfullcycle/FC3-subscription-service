using Subscription.Domain.Enum;

namespace Subscription.Application.Adapters.PaymentGateway
{
    public interface IPaymentGateway
    {
        Task<bool> MakePaymentAsync(PaymentInput input, CancellationToken cancellationToken);
    }

    public class PaymentInput
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Doc { get; set; }
        public decimal Value { get; set; }
        public PaymentType PaymentType { get; set; }

        public static PaymentInput CreatePix(string name, string lastName, string doc, decimal value)
        {
            return new PaymentInput
            {
                Name = name,
                LastName = lastName,
                Doc = doc,
                Value = value,
                PaymentType = PaymentType.Pix
            };
        }
        public static PaymentInput CreateCreditCard(string name, string lastName, string doc, decimal value)
        {
            return new PaymentInput
            {
                Name = name,
                LastName = lastName,
                Doc = doc,
                Value = value,
                PaymentType = PaymentType.CreditCard
            };
        }
    }
}
