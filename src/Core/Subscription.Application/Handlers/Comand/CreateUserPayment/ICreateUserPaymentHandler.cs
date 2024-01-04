using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Comand.CreateUserPayment
{
    public interface ICreateUserPaymentHandler : IBaseHandler<CreateUserPaymentInput, CreateUserPaymentOutput>
    {}
}
