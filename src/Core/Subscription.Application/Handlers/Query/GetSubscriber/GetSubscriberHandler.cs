using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;

namespace Subscription.Application.Handlers.Query.GetSubscriber
{
    public class GetSubscriberHandler : BaseHandler<GetSubscriberInput, GetSubscriberOutput>, IGetSubscriberHandler
    {
        private readonly ISubscriptionRepository _repository;

        public GetSubscriberHandler(ILogger<GetSubscriberInput> logger, ISubscriptionRepository repository) : 
            base(logger, new GetSubscriberInputValidator())
        {
            _repository = repository;
        }
        public async override Task Handle(GetSubscriberInput input, CancellationToken cancellationToken)
        {
            LogInformation("Getting subscriber");

            var response = await _repository.GetSubscriberById(input.SubscriberId, cancellationToken);

            Response.AddResult(response);

            LogInformation("Getted subscriber");
        }
    }
}
