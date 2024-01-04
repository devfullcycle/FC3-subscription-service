using Subscription.Domain.Abstractions.Base;

namespace Subscription.Domain.Entities
{
    public class PlanCost : EntityBase
    {
        public decimal Price { get; set; }
        public Guid PlanId { get; set; }
        public bool IsActive { get; set; }


        public void ChangePrice(decimal price)
        {
            if(price < 0 || price == 0)
            {
                AddNotification("Preço invalido, deve ser maior que zero");
                return;
            }
            Price = price;
        }

        public void ChangePlanId(Guid planId)
        {
            if (Guid.Empty == planId)
            {
                AddNotification("planId should be informed");
                return;
            }
            PlanId = planId;
        }

        public void ChangePlanCostId(Guid plancostId)
        {
            if (Guid.Empty == plancostId)
            {
                AddNotification("planCostId should be informed");
                return;
            }
            Id = plancostId;
        }

        public static PlanCost Create(decimal value, Guid planId, Guid? id = null, bool? isActive = true)
        {
            PlanCost planCost = new();
             if (id is not null)
                planCost.ChangePlanCostId(id.Value);
            planCost.ChangePrice(value);
            planCost.ChangePlanId(planId);
            planCost.IsActive = isActive.Value;
            return planCost;
        }

        public static PlanCost CreateBlank() => new();

        public static PlanCost CreateWithErrorMessage(string message)
        {
            var plancost = new PlanCost();
            plancost.AddNotification(message);
            return plancost;
        }
    }
}
