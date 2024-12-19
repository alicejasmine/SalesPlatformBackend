namespace Api.Service.Credits.DTOs;

  public class CreditHistoryResponse
    {
        public string InvoiceNumber { get; set; }
        public string OrganizationAlias { get; set; }
        public Guid OrganizationId { get; set; }
        public decimal TotalPartnershipCredits { get; set; }
        public decimal CreditsSpent { get; set; }
        public decimal RemainingCredits { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }