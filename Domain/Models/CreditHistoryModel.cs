namespace Domain.Models;

public class CreditHistoryModel : BaseModel
{
    public CreditHistoryModel(Guid id, DateTime created, DateTime modified, int invoiceNumber, int partnershipCredits, int creditsSpend, int newTotalCredits, DateOnly creditStart, DateOnly creditEnd)
        : base(id, created, modified)
    {
        InvoiceNumber = invoiceNumber;
        PartnershipCredits = partnershipCredits;
        CreditsSpend = creditsSpend;
        NewTotalCredits = newTotalCredits;
        CreditStart = creditStart;
        CreditEnd = creditEnd;
    }

    public int InvoiceNumber { get; set; }
    public int PartnershipCredits { get; set; }
    public int CreditsSpend { get; set; }
    public int NewTotalCredits { get; set; }
    public DateOnly CreditStart { get; set; }
    public DateOnly CreditEnd { get; set; }
}
