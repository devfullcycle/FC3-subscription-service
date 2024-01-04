using Microsoft.AspNetCore.Mvc;
using Subscription.Application.Handlers.Comand.CreateUser;
using Subscription.Application.Handlers.Comand.CreateUserPayment;
using Subscription.Application.Handlers.Query.GetSubscriber;
using Subscription.Application.Handlers.Query.GetSusbscribers;

namespace Subscription.API.Routes
{
    public static class UsersRoute
    {
        public static IEndpointRouteBuilder RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var plansRoute = routes.MapGroup("api/v1/user");

            plansRoute.MapPost("/create", async ([FromServices] ICreateUserHandler createUser,
                                    [FromBody] CreateUserInput input,
                                    CancellationToken cancellation) =>
            {
                return await createUser.HandleExecutionAsync(input, cancellation);
            }).WithDescription("Create user");

            plansRoute.MapPost("/subscription", async ([FromServices] ICreateUserPaymentHandler createUserSub,
                                    [FromBody] CreateUserPaymentInput input,
                                    CancellationToken cancellation) =>
            {
                return await createUserSub.HandleExecutionAsync(input, cancellation);
            }).WithDescription("Create user Subscription");

            plansRoute.MapGet("", async ([FromServices] IGetSubscribersHandler createUserSub,
                                    [FromQuery] int page, [FromQuery]int size,
                                    CancellationToken cancellation) =>
            {
                var input = new GetSubscribersInput(page, size, Guid.NewGuid());

                return await createUserSub.HandleExecutionAsync(input, cancellation);
            }).WithDescription("Create user Subscription");

            plansRoute.MapGet("subscriber/{Id}", async ([FromServices] IGetSubscriberHandler handler,
                                    [FromRoute]Guid Id,
                                    CancellationToken cancellation) =>
            {
                var input = new GetSubscriberInput(Id, Guid.NewGuid());

                return await handler.HandleExecutionAsync(input, cancellation);
            }).WithDescription("Create user Subscription");
            return routes;
        }
    }
}
