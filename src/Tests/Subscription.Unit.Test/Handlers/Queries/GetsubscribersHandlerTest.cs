using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Query.GetSusbscribers;

namespace Subscription.Unit.Test.Handlers.Queries
{
    public class GetsubscribersHandlerTest
    {
        [Fact]
        public async Task GetSubscribersWithSuccess()
        {
            var input = new GetSubscribersInput(1, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscribersInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscribersOutput.Create();

            data.AddSubscriber(GetSubscribersResumeOutput
                                .GetInstance()
                                .AddPlan("plan1", 100)
                                .AddName("Name", "lastname", true));
            var metadata = new MetaData();
            metadata.AddPagination(1, 1, 1);

            repository
                .Setup(item => item.GetSubscribers(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(data);
            repository
                .Setup(item => item.GetSubscribersPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(metadata);

            var handler = new GetSubscribersHandler(logger.Object, repository.Object);

            var result = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Meta.IsValid);
        }

        [Fact]
        public async Task GetSubscribersWithInvalidInput()
        {
            var input = new GetSubscribersInput(0, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscribersInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscribersOutput.Create();

            data.AddSubscriber(GetSubscribersResumeOutput
                                .GetInstance()
                                .AddPlan("plan1", 100)
                                .AddName("Name", "lastname", true));
            var metadata = new MetaData();
            metadata.AddPagination(1, 1, 1);

            repository
                .Setup(item => item.GetSubscribers(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(data);
            repository
                .Setup(item => item.GetSubscribersPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(metadata);

            var handler = new GetSubscribersHandler(logger.Object, repository.Object);

            var result = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Meta.IsValid);
        }

        [Fact]
        public async Task GetSubscribersWithThrowsInput()
        {
            var input = new GetSubscribersInput(1, 1, Guid.NewGuid());
            var logger = new Mock<ILogger<GetSubscribersInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var data = GetSubscribersOutput.Create();

            data.AddSubscriber(GetSubscribersResumeOutput
                                .GetInstance()
                                .AddPlan("plan1", 100)
                                .AddName("Name", "lastname", true));
            var metadata = new MetaData();
            metadata.AddPagination(1, 1, 1);

            repository
                .Setup(item => item.GetSubscribers(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(data);
            repository
                .Setup(item => item.GetSubscribersPaginated(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Some error ocurred"));

            var handler = new GetSubscribersHandler(logger.Object, repository.Object);

            var result = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Meta.IsValid);
        }
    }
}
