using Domain.Enum;

namespace Domain.Entities;
public class OrganizationEntity : BaseEntity
{
    public string Alias { get; set; }
    public string DisplayName { get; set; }
    public int TotalCredits { get; private set; }
    public PartnershipEnum Partnership { get; set; }
    public ICollection<ProjectEntity>? Projects { get; set; }

    // future implemntation
    //public ICollection<PurchaseModel>? Purchases { get; set; }
    //public ICollection<CreditHistoryModel>? CreditHistory { get; set; }
}