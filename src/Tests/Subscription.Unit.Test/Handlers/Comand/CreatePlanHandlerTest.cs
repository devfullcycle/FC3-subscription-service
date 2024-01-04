using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Comand.CreatePlan;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;

namespace Subscription.Unit.Test.Handlers.Comand
{
    public class CreatePlanHandlerTest
    {
        [Fact]
        public async Task CreatePlanWithSuccess()
        {
            var input = new CreatePlanInput("plan1", "plan1 e o plan1", PlanType.MONTHLY, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanInput>>();
            var respository = new Mock<ISubscriptionRepository>();
            var response = Plan.CreateMonthly(input.Name, input.Description);
            response.Id = Guid.NewGuid();

            var output = new Output<CreatePlanOutput>();
            output.AddResult(CreatePlanOutput.Create(response.Id));

            respository
                .Setup(item => item.CreatePlanAsync(It.IsAny<Plan>(), CancellationToken.None))
                .ReturnsAsync(response);

            var handler = new CreatePlanHandler(logger.Object, respository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.True(itemToAssert.Meta.IsValid);

        }

        [Fact]
        public async Task CreatePlanWithInvalidInput()
        {
            var input = new CreatePlanInput("pl", "plan1 e o plan1", PlanType.MONTHLY, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanInput>>();
            var respository = new Mock<ISubscriptionRepository>();
            var response = Plan.CreateMonthly(input.Name, input.Description);
            response.Id = Guid.NewGuid();

            var output = new Output<CreatePlanOutput>();
            output.AddResult(CreatePlanOutput.Create(response.Id));

            respository
                .Setup(item => item.CreatePlanAsync(It.IsAny<Plan>(), CancellationToken.None))
                .ReturnsAsync(response);

            var handler = new CreatePlanHandler(logger.Object, respository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(itemToAssert.Meta.IsValid);

        }

        [Fact]
        public async Task CreatePlanWithSuccessAnually()
        {
            var input = new CreatePlanInput("plan1", "plan1 e o plan1", PlanType.ANNUALLY, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanInput>>();
            var respository = new Mock<ISubscriptionRepository>();
            var response = Plan.CreateAnnually(input.Name, input.Description);
            response.Id = Guid.NewGuid();

            var output = new Output<CreatePlanOutput>();
            output.AddResult(CreatePlanOutput.Create(response.Id));

            respository
                .Setup(item => item.CreatePlanAsync(It.IsAny<Plan>(), CancellationToken.None))
                .ReturnsAsync(response);

            var handler = new CreatePlanHandler(logger.Object, respository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.True(itemToAssert.Meta.IsValid);

        }

        [Fact]
        public async Task CreatePlanWithRepositoryTrow()
        {
            var input = new CreatePlanInput("plan1", "plan1 e o plan1", PlanType.ANNUALLY, Guid.NewGuid());
            var logger = new Mock<ILogger<CreatePlanInput>>();
            var respository = new Mock<ISubscriptionRepository>();
            var response = Plan.CreateAnnually(input.Name, input.Description);
            response.Id = Guid.NewGuid();

            var output = new Output<CreatePlanOutput>();
            output.AddResult(CreatePlanOutput.Create(response.Id));

            respository
                .Setup(item => item.CreatePlanAsync(It.IsAny<Plan>(), CancellationToken.None))
                .Throws(new Exception("Some error ocurred"));

            var handler = new CreatePlanHandler(logger.Object, respository.Object);

            var itemToAssert = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(itemToAssert.Meta.IsValid);

        }

    }
}
