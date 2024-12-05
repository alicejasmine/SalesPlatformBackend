using Domain.Enum;

namespace Domain.Models;

public class OrganizationModel : BaseModel
{
    public OrganizationModel(Guid id, string alias, string displayName, int totalCredits, PartnershipEnum partnership, ICollection<ProjectModel> projects, ICollection<PurchaseModel> purchases, ICollection<CreditHistoryModel> creditHistory, DateTime created, DateTime modified)
        : base(id, created, modified)
    {
        Alias = alias;
        DisplayName = displayName;
        TotalCredits = totalCredits;
        Partnership = partnership;
        Projects = projects ?? new List<ProjectModel>();
        Purchases = purchases ?? new List<PurchaseModel>();
        CreditHistory = creditHistory ?? new List<CreditHistoryModel>();
    }

    public string Alias { get; set; }
    public string DisplayName { get; set; }
    public int TotalCredits { get; private set; }
    public PartnershipEnum Partnership { get; set; }
    public ICollection<ProjectModel> Projects { get; set; }
    public ICollection<PurchaseModel> Purchases { get; set; }
    public ICollection<CreditHistoryModel> CreditHistory { get; set; }
}