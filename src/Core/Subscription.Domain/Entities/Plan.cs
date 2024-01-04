using Subscription.Domain.Abstractions.Base;
using Subscription.Domain.Enum;

namespace Subscription.Domain.Entities
{
    public class Plan : EntityBase
    {
        public Plan()
        {}

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public PlanType PlanType { get; set; }
        public List<PlanCost> PlanCosts { get; set; } = new();

        public void ChangeName(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                AddNotification("Nome do plano incompleto, minimo 3 caracteres");
                return;
            }
            Name = name;
        }

        public void ChangeDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description) || description.Length < 3)
            {
                AddNotification("Descricao do plano incompleta, minimo 3 caracteres");
                return;
            }
            Description = description;
        }

        public static Plan CreateMonthly(string name, string description)
        {
            Plan plan = new Plan();
            plan.Id = Guid.NewGuid();
            plan.ChangeName(name);
            plan.ChangeDescription(description);
            plan.PlanType = PlanType.MONTHLY;
            return plan;
        }

        public static Plan CreateAnnually(string name, string description)
        {
            Plan plan = new Plan();
            plan.Id = Guid.NewGuid();
            plan.ChangeName(name);
            plan.ChangeDescription(description);
            plan.PlanType = PlanType.ANNUALLY;
            return plan;
        }

        public static Plan CreateWithErrorMessage(string message)
        {
            var plancost = new Plan();
            plancost.AddNotification(message);
            return plancost;
        }


        public void AddPlanCost(PlanCost planCost)
        {
            if (!planCost.IsValid)
            {
                AddNotifications(planCost.Notifications);
                planCost.ClearNotifications();
                return;
            }

            PlanCosts.Add(planCost);
        } 
    }
}
