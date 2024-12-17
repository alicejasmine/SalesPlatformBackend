namespace Domain.Models;

public class CreditHistoryModel : BaseModel
{
    public CreditHistoryModel(Guid id, DateTime created, DateTime modified, int invoiceNumber, int partnershipCredits, 
        int creditsSpend, int currentCredits, DateOnly creditStart, DateOnly creditEnd, Guid organizationId)
        : base(id, created, modified)
    {
        InvoiceNumber = invoiceNumber;
        PartnershipCredits = partnershipCredits;
        CreditsSpend = creditsSpend;
        CurrentCredits = currentCredits;
        CreditStart = creditStart;
        CreditEnd = creditEnd;
        OrganizationId = organizationId;
    }

    public int InvoiceNumber { get; set; }
    public int PartnershipCredits { get; set; }
    public int CreditsSpend { get; set; }
    public int CurrentCredits { get; set; }
    public DateOnly CreditStart { get; set; }
    public DateOnly CreditEnd { get; set; }
    public Guid OrganizationId { get; set; } 
}
