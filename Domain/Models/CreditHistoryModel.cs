namespace Domain.Models;

public class CreditHistoryModel : BaseModel
{
    public CreditHistoryModel(Guid id, DateTime created, DateTime modified, string invoiceNumber, decimal partnershipCredits, 
        decimal creditsSpend, decimal currentCredits, Guid organizationId)
        : base(id, created, modified)
    {
        InvoiceNumber = invoiceNumber;
        PartnershipCredits = partnershipCredits;
        CreditsSpend = creditsSpend;
        CurrentCredits = currentCredits;
        OrganizationId = organizationId;
    }

    public string InvoiceNumber { get; set; }
    public decimal PartnershipCredits { get; set; }
    public decimal CreditsSpend { get; set; }
    public decimal CurrentCredits { get; set; } 
    public Guid OrganizationId { get; set; } 
}
