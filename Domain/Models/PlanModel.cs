using Domain.Enum;

namespace Domain.Models;

public class PlanModel : BaseModel
{
    public PlanModel(Guid id, string name, int priceInDKK, PlanEnum plan, DateTime created, DateTime modified)
        : base(id, created, modified)
    {
        Name = name;
        PriceInDKK = priceInDKK;
        Plan = plan;
    }

    public PlanEnum Plan { get; set; }
    public int PriceInDKK { get; set; }
    public string Name { get; set; }
}