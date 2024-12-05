using Domain.Enum;

namespace Domain.Entities;
public class PlanEntity : BaseEntity
{
    public PlanEnum Plan { get; set; }
    public int PriceInDKK { get; set; }
}