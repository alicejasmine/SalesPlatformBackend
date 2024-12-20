using Domain.Enum;

namespace Domain.Models;

public class OrganizationModel : BaseModel
{
    public OrganizationModel(Guid id, string alias, string displayName, int totalCredits, PartnershipEnum partnership,
        DateTime created, DateTime modified, ICollection<CreditHistoryModel>? creditHistory)
        : base(id, created, modified)
    {
        Alias = alias;
        DisplayName = displayName;
        TotalCredits = totalCredits;
        Partnership = partnership;
        CreditHistory = creditHistory ?? new List<CreditHistoryModel>();
    }

    public string Alias { get; set; }
    public string DisplayName { get; set; }
    public int TotalCredits { get; private set; }
    public PartnershipEnum Partnership { get; set; }
    public ICollection<ProjectModel> Projects { get; set; }
    public ICollection<CreditHistoryModel>? CreditHistory { get; set; }
}