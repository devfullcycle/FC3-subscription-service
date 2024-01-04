using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Comand.CreatePlanCost;
using Subscription.Domain.Entities;

namespace Subscription.Unit.Test.Handlers.Comand
{
    public class CreatePlanCostHandlerTest
    {
        [Fact]
        public async Task CreatePlanCostWithSuccess()
        {
            var input = new CreatePlanCostInput(Guid.NewGuid(), 100, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanCostInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var response = PlanCost.Create(input.Price, input.PlanId);
            var output = new Output<CreatePlanCostOutput>();
            output.AddResult(CreatePlanCostOutput.Create(response.Id));

            repository
                .Setup(item => item.CreatePlanCostAsync(It.IsAny<PlanCost>(), CancellationToken.None))
                .ReturnsAsync(response);
            repository
                .Setup(item => item.GetPlanByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(new Plan() { Id = input.PlanId });

            var handler = new CreatePlanCostHandler(logger.Object, repository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(itemToAssert);
            Assert.True(itemToAssert.Meta.IsValid);
        }

        [Fact]
        public async Task CreatePlanCostWithInvalidInput()
        {
            var input = new CreatePlanCostInput(Guid.NewGuid(), 0, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanCostInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var response = PlanCost.Create(input.Price, input.PlanId);
            var output = new Output<CreatePlanCostOutput>();
            output.AddResult(CreatePlanCostOutput.Create(response.Id));

            repository
                .Setup(item => item.CreatePlanCostAsync(It.IsAny<PlanCost>(), CancellationToken.None))
                .ReturnsAsync(response);

            var handler = new CreatePlanCostHandler(logger.Object, repository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(itemToAssert);
            Assert.False(itemToAssert.Meta.IsValid);
        }

        [Fact]
        public async Task CreatePlanCostWithRepositoryError()
        {
            var input = new CreatePlanCostInput(Guid.NewGuid(), 0, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanCostInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var response = PlanCost.Create(input.Price, input.PlanId);
            var output = new Output<CreatePlanCostOutput>();
            output.AddResult(CreatePlanCostOutput.Create(response.Id));

            repository
                .Setup(item => item.CreatePlanCostAsync(It.IsAny<PlanCost>(), CancellationToken.None))
                .Throws(new Exception("Some error"));

            var handler = new CreatePlanCostHandler(logger.Object, repository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(itemToAssert);
            Assert.False(itemToAssert.Meta.IsValid);
        }
    }
}
