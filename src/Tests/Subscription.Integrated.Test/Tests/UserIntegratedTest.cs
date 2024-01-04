using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Adapters.Repository;
using Subscription.Domain.Entities;
using Subscription.Domain.ValueObject;
using Subscription.Integrated.Test.Configuration;

namespace Subscription.Integrated.Test.Tests
{
    public class UserIntegratedTest : IClassFixture<ApplicationConfiguration>
    {
        private readonly ApplicationConfiguration _factory;

        public UserIntegratedTest(ApplicationConfiguration factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "integratedTest");
        }

        [Fact]
        public async Task CreateUserWithSucces()
        {
            using var scope = _factory.Services.CreateScope();
            _factory.Init(scope);
            await _factory.CreateDatabase();

            var _repository = _factory.Services.GetRequiredService<ISubscriptionRepository>();

            var addres = Address.Create("12345678", "street", "city", "Sp", "BR");
            var doc = Document.Create("12345678901");
            var input = User.Create("User", "LasName", 20, addres, doc);

            var response  = await _repository.CreateUserOnDatabaseasync(input, CancellationToken.None);

            response.Id.Should().NotBe(Guid.Empty);

            await _factory.DropDatabaseTables();
        }

        [Fact]
        public async Task GetUserByDocumentNumber()
        {

            using var scope = _factory.Services.CreateScope();
            _factory.Init(scope);
            await _factory.CreateDatabase();

            var _repository = _factory.Services.GetRequiredService<ISubscriptionRepository>();

            var addres = Address.Create("12345678", "street", "city", "Sp", "BR");
            var doc = Document.Create("12345678901");
            var input = User.Create("User", "LasName", 20, addres, doc);

            await _repository.CreateUserOnDatabaseasync(input, CancellationToken.None);

            var response = await _repository.GetUserByDocumentNumber(input.Document.DocumentNumber, CancellationToken.None);

            response.Id.Should().NotBe(Guid.Empty);

            await _factory.DropDatabaseTables();
        }


        [Fact]
        public async Task GetSubscribers()
        {
            using var scope = _factory.Services.CreateScope();
            _factory.Init(scope);
            await _factory.CreateDatabase();

            var _repository = _factory.Services.GetRequiredService<ISubscriptionRepository>();

            var addres = Address.Create("12345678", "street", "city", "Sp", "BR");
            var doc = Document.Create("12345678901");
            var input = User.Create("User", "LasName", 20, addres, doc);
            var plan = Plan.CreateAnnually("plano", "plano é o melhor plano");

            var user  = await _repository.CreateUserOnDatabaseasync(input, CancellationToken.None);
            var plancreated = await _repository.CreatePlanAsync(plan, CancellationToken.None);

            plancreated.AddPlanCost(PlanCost.Create(100, plancreated.Id));

            await _repository.CreatePlanCostAsync(plancreated.PlanCosts[0], CancellationToken.None);

            var response = await _repository.CreateSubscriptionOnDatabaseasync(UserSubscription.Create(user, plancreated, DateTime.Now), CancellationToken.None);

            response.Id.Should().NotBe(Guid.Empty);

            await _factory.DropDatabaseTables();
        }

        [Fact]
        public async Task GetPlanPaginated() { }
    }
}
