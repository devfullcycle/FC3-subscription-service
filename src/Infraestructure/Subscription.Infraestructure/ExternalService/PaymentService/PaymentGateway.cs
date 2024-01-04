using Subscription.Application.Adapters.PaymentGateway;

namespace Subscription.Infraestructure.ExternalService.PaymentService
{
    public class PaymentGateway : IPaymentGateway
    {
        public async Task<bool> MakePaymentAsync(PaymentInput input, CancellationToken cancellationToken)
        {
			try
			{
				await Task.Delay(1200, cancellationToken);
				return true;
			}
			catch
			{
				return false;
			}
        }
    }
}
