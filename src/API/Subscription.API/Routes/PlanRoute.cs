using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription.Application.Handlers.Comand.CreatePlan;
using Subscription.Application.Handlers.Comand.CreatePlanCost;
using Subscription.Application.Handlers.Query.GetPlans;

namespace Subscription.API.Routes
{
    [Authorize]
    public static class PlanRoute
    {
        public static IEndpointRouteBuilder RegisterPlanEndpoints(this IEndpointRouteBuilder routes)
        {
            var plansRoute = routes.MapGroup("api/v1/plan");

            plansRoute.MapPost("/create", async ([FromServices] ICreatePlanHandler createPlan,
                                    [FromBody] CreatePlanInput input,
                                    CancellationToken cancellation) =>
            {
                return await createPlan.HandleExecutionAsync(input, cancellation);
            }).WithDescription("Create Plan").RequireAuthorization("manage");

            plansRoute.MapPost("/plancost", async ([FromServices] ICreatePlanCostHandler handler, [FromBody] CreatePlanCostInput input, CancellationToken cancellationToken) =>
            {
                return await handler.HandleExecutionAsync(input, cancellationToken);

            }).WithDescription("Create PlanCost").RequireAuthorization("manage");

            plansRoute.MapGet("", async ([FromServices] IGetPlansHandler handler, [FromQuery] int page, [FromQuery] int size, CancellationToken cancellationToken) =>
            {
                var input = new GetPlansInput(page,size, Guid.NewGuid());
                return await handler.HandleExecutionAsync(input, cancellationToken);

            }).WithDescription("Get Plans");

            return routes;
        }
    }
}
