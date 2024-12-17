using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class CreditHistoryEntity : BaseEntity
{  
        [Required]
        public Guid OrganizationId { get; set; } 
        [Required]
        public OrganizationEntity Organization { get; set; }
        [Required]
        public int InvoiceNumber { get; set; }
        [Required]
        public int PartnershipCredits { get; set; }
        [Required]
        public int CreditsSpend { get; set; }
        [Required]
        public int CurrentCredits { get; set; }
        [Required]
        public DateOnly CreditStart { get; set; }
        [Required]
        public DateOnly CreditEnd { get; set; }
}