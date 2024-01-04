using Subscription.Domain.Entities;

namespace Subscription.Unit.Test.Entities
{
    public class PlanCostUnitTest
    {
        [Fact]
        public void CreatePlanCost()
        {
            var planId = Guid.NewGuid();
            var plancost = PlanCost.Create(1, planId);
            Assert.Equal(planId, plancost.PlanId);
            Assert.True(plancost.IsValid);
        }

        [Fact]
        public void CreatePlanCostWrongValue()
        {
            var plancost = PlanCost.Create(0, Guid.NewGuid());
            Assert.False(plancost.IsValid);
            Assert.Equal("Preço invalido, deve ser maior que zero", plancost.Notifications.First());
        }

        [Fact]
        public void CreatePlanCostWithMessaError()
        {
            var plancost = PlanCost.CreateWithErrorMessage("Erro de criacao");
            Assert.False(plancost.IsValid);
            Assert.Equal("Erro de criacao", plancost.Notifications.First());
        }

        [Fact]
        public void CreatePlanCostWithPlanIdEmpty()
        {
            var plancost = PlanCost.Create(1, Guid.Empty);
            Assert.False(plancost.IsValid);
            Assert.Equal("planId should be informed", plancost.Notifications.First());
        }

        [Fact]
        public void CreatePlanCostWithWrongCost()
        {
            var plancost = PlanCost.Create(0, Guid.NewGuid());
            Assert.False(plancost.IsValid);
            Assert.Equal("Preço invalido, deve ser maior que zero", plancost.Notifications.First());
        }

        [Fact]
        public void CreatePlanCostWithWrongPlanCostId()
        {
            var plancost = PlanCost.Create(1, Guid.NewGuid());
            plancost.ChangePlanCostId(Guid.Empty);
            Assert.False(plancost.IsValid);
            Assert.Equal("planCostId should be informed", plancost.Notifications.First());
        }

        [Fact]
        public void CreatePlanCostWitPlanCostId()
        {
            var plancost = PlanCost.Create(1, Guid.NewGuid(), id: Guid.NewGuid());
            Assert.True(plancost.IsValid);
        }
    }
}
