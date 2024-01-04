using Subscription.Application.Abstractions.Handlers;

namespace Subscription.Application.Handlers.Comand.CreateUser
{
    public interface ICreateUserHandler : IBaseHandler<CreateUserInput, CreateUserOutput>
    {
    }
}
