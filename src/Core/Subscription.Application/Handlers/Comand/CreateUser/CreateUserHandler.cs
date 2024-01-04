using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;

namespace Subscription.Application.Handlers.Comand.CreateUser
{
    public class CreateUserHandler :
        BaseHandler<CreateUserInput, CreateUserOutput>,
        ICreateUserHandler
    {
        private readonly ISubscriptionRepository _repository;

        public CreateUserHandler(ILogger<CreateUserInput> logger, ISubscriptionRepository repository):
            base(logger, new CreateuserInputValidator())
        {
            _repository = repository;
        }
        public async override Task Handle(CreateUserInput input, CancellationToken cancellationToken)
        {
            LogInformation("Saving user on database");

            var user = await _repository.GetUserByDocumentNumber(input.DocumentNumber, cancellationToken);

            if (user is not null && !user.Id.Equals(Guid.Empty))
            {
                Response.AddMessage("Document number already exists");
                return;
            }

            var response = await _repository.CreateUserOnDatabaseasync(input.CreaUser(), cancellationToken);

            LogInformation("Saved user on database");

            Response.AddResult(CreateUserOutput.Create(response.Id));
        }
    }
}
