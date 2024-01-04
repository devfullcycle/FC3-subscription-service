using Subscription.Application.Handlers.Comand.CreatePlanCost;

namespace Subscription.Application.Handlers.Comand.CreateUser
{
    public class CreateUserOutput
    {
        public Guid Id { get; private set; }
        public static CreateUserOutput Create(Guid id)
        {
            CreateUserOutput userOutput = new()
            {
                Id = id
            };
            return userOutput;
        }
    }
}
