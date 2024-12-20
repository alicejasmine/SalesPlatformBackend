using System.Collections;
using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Entities;
public class OrganizationEntity : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Alias { get; set; }

    [Required]
    [MaxLength(50)]
    public string DisplayName { get; set; }

    [Required]
    public int TotalCredits { get; private set; }

    [Required]
    public PartnershipEnum Partnership { get; private set; }

    [Required]
    public ICollection<ProjectEntity>? Projects { get; set; }
    
    [Required]
    public ICollection<CreditHistoryEntity>? CreditHistories{ get; set; }
    // public IEnumerable<PurchaseEntity>? Purchases { get; set; }

    public OrganizationEntity(Guid id, string alias, string displayName, int totalCredits, PartnershipEnum partnership, DateTime created, DateTime modified, ICollection<CreditHistoryEntity>? creditHistories) : base(id, created, modified)
    {
        Alias = alias;
        DisplayName = displayName;
        TotalCredits = totalCredits;
        Partnership = partnership;
        CreditHistories = creditHistories;
    }

    public OrganizationEntity()
    {
    }
}