using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;

namespace Subscription.Application.Handlers.Query.GetSusbscribers
{
    public class GetSubscribersHandler : BaseHandler<GetSubscribersInput, GetSubscribersOutput>, IGetSubscribersHandler
    {
        private readonly ISubscriptionRepository _repository;

        public GetSubscribersHandler( ILogger<GetSubscribersInput> logger, ISubscriptionRepository repository)
            : base(logger, new GetSubscribersInputValidator())
        {
            _repository = repository;
        }
        public async override Task Handle(GetSubscribersInput input, CancellationToken cancellationToken)
        {
            LogInformation("Getting Subscribers");

            var subscribers = await _repository.GetSubscribers(input.Page, input.Size, cancellationToken);
            var meta = await _repository.GetSubscribersPaginated(input.Page, input.Size, cancellationToken);

            Response.Meta.AddPagination(meta.PageSize, meta.Page, meta.Total);
            Response.Meta.AddLink(input.GetHostUrl() + "/{id:int}");
            Response.AddResult(subscribers);

            LogInformation("Getted subscribers");
        }
    }
}
