using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Query.GetSubscriber;
using Subscription.Domain.Enum;

namespace Subscription.Unit.Test.Handlers.Queries
{
    public class GetSubscriberHandlerTest
    {
        [Fact]
        public async Task GetSubscriberWithSuccss()
        {
            var input = new GetSubscriberInput(Guid.NewGuid(), Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscriberInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscriberOutput
                            .Create()
                            .WithUser("name", "LastName",26)
                            .WithDocument("12345678901")
                            .WithAddress("12345678", "city", "BR", "SP", "street")
                            .WithPlan("plan1", "plan 1 é o plan 1", 100, PlanType.MONTHLY);

            repository
                .Setup(item => item.GetSubscriberById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(data);

            var handler = new GetSubscriberHandler(logger.Object, repository.Object);

            var reseponse = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(reseponse);
            Assert.True(reseponse.Meta.IsValid);
        }

        [Fact]
        public async Task GetSubscriberWithInvalidInput()
        {
            var input = new GetSubscriberInput(Guid.Empty, Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscriberInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscriberOutput
                            .Create()
                            .WithUser("name", "LastName",26)
                            .WithDocument("12345678901")
                            .WithAddress("12345678", "city", "BR", "SP", "street")
                            .WithPlan("plan1", "plan 1 é o plan 1", 100, PlanType.MONTHLY);

            repository
                .Setup(item => item.GetSubscriberById(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(data);

            var handler = new GetSubscriberHandler(logger.Object, repository.Object);

            var reseponse = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(reseponse);
            Assert.False(reseponse.Meta.IsValid);
        }

        [Fact]
        public async Task GetSubscriberWithThrowsInput()
        {
            var input = new GetSubscriberInput(Guid.NewGuid(), Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscriberInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscriberOutput
                            .Create()
                            .WithUser("name", "LastName",26)
                            .WithDocument("12345678901")
                            .WithAddress("12345678", "city", "BR", "SP", "street")
                            .WithPlan("plan1", "plan 1 é o plan 1", 100, PlanType.MONTHLY);

            repository
                .Setup(item => item.GetSubscriberById(It.IsAny<Guid>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Some error ocurred"));

            var handler = new GetSubscriberHandler(logger.Object, repository.Object);

            var reseponse = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(reseponse);
            Assert.False(reseponse.Meta.IsValid);
        }
    }
}
