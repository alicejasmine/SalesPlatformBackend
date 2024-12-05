using Domain.Enum;

namespace Domain.Entities;
public class PlanEntity : BaseEntity
{
    public string Name { get; set; }
    public PlanEnum Plan { get; set; }
    public int PriceInDKK { get; set; }
}