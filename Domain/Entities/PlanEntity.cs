using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class PlanEntity : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public PlanEnum Plan { get; set; }

    [Required]
    public int PriceInDKK { get; set; }

    public PlanEntity(Guid id, string name, int priceInDKK, PlanEnum plan, DateTime created, DateTime modified) : base(id, created, modified)
    {
        Name = name;
        PriceInDKK = priceInDKK;
        Plan = plan;
    }
}