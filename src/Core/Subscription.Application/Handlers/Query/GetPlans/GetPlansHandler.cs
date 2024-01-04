using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;

namespace Subscription.Application.Handlers.Query.GetPlans
{
    public class GetPlansHandler : BaseHandler<GetPlansInput, GetPlansOutput>, IGetPlansHandler
    {
        private readonly ISubscriptionRepository _repository;

        public GetPlansHandler(ILogger<GetPlansInput> logger, ISubscriptionRepository repository) : 
            base(logger, new GetPlansInputValidator())
        {
            _repository = repository;
        }
        public async override Task Handle(GetPlansInput input, CancellationToken cancellationToken)
        {
            LogInformation("Getting plans");

            var plansObject = await _repository.GetPlans(input.Page, input.Size, cancellationToken);
            var metaData = await _repository.GetPlansPaginated(input.Page, input.Size, cancellationToken);

            Response.Meta.AddPagination(metaData.PageSize, metaData.Page, metaData.Total);
            Response.Meta.AddLink(input.GetHostUrl() + "/{id:int}");
            Response.AddResult(GetPlansOutput.Create(plansObject));

            LogInformation("PLans Getted");
        }
    }
}
