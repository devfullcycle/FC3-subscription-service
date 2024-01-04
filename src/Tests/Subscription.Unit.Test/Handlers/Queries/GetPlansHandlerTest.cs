using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Query.GetPlans;
using Subscription.Domain.Entities;

namespace Subscription.Unit.Test.Handlers.Queries
{
    public class GetPlansHandlerTest
    {
        [Fact] 
        public async Task GetPlanWithSuccess()
        {
            var input = new GetPlansInput(1, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetPlansInput>>();
            var reposoty = new Mock<ISubscriptionRepository>();

            reposoty
                .Setup(item => item.GetPlans(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(new List<Plan>() { Plan.CreateMonthly("plano1", "plano 1 é o plano 1") });

            var meta = new MetaData();
            meta.AddPagination(1, 1, 1);

            reposoty
                .Setup(item => item.GetPlansPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(meta);

            var handler = new GetPlansHandler(logger.Object, reposoty.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.True(response.Meta.IsValid);
        }

        [Fact]
        public async Task GetPlanWithInvalidInput()
        {
            var input = new GetPlansInput(0, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetPlansInput>>();
            var reposoty = new Mock<ISubscriptionRepository>();

            reposoty
                .Setup(item => item.GetPlans(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(new List<Plan>() { Plan.CreateMonthly("plano1", "plano 1 é o plano 1") });

            var meta = new MetaData();
            meta.AddPagination(1, 1, 1);

            reposoty
                .Setup(item => item.GetPlansPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(meta);

            var handler = new GetPlansHandler(logger.Object, reposoty.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(response.Meta.IsValid);
        }

        [Fact]
        public async Task GetPlanWithThrowsInput()
        {
            var input = new GetPlansInput(1, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetPlansInput>>();
            var reposoty = new Mock<ISubscriptionRepository>();

            reposoty
                .Setup(item => item.GetPlans(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(new List<Plan>() { Plan.CreateMonthly("plano1", "plano 1 é o plano 1") });

            var meta = new MetaData();
            meta.AddPagination(1, 1, 1);

            reposoty
                .Setup(item => item.GetPlansPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Some error ocurred"));

            var handler = new GetPlansHandler(logger.Object, reposoty.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(response.Meta.IsValid);
        }
    }
}
