using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class CreditHistoryEntity : BaseEntity
{  
        [Required]
        public Guid OrganizationId { get; set; } 
      
        [Required]
        [MaxLength(8)]
        public string InvoiceNumber { get; set; }
        [Required]
        public decimal PartnershipCredits { get; set; }
        [Required]
        public decimal CreditsSpend { get; set; }
        [Required]
        public decimal CurrentCredits { get; set; }
        public OrganizationEntity Organization { get; set; }
}