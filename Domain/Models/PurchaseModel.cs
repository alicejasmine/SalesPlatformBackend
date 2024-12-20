using Domain.Enum;

namespace Domain.Models;

public class PurchaseModel : BaseModel
{
    public PurchaseModel(Guid id, DateTime created, DateTime modified, PlanEnum plan, int purchasedAmount, string comment)
        : base(id, created, modified)
    {
        Plan = plan;
        PurchasedAmount = purchasedAmount;
        Comment = comment;
    }

    public PlanEnum Plan { get; set; }
    public int PurchasedAmount { get; set; }
    public string Comment { get; set; }
}