using System.ComponentModel.DataAnnotations;

namespace Api.Service.DTOs;

public class GetMonthlyUsageRequestDto
{
        [Required] 
        public Guid EnvironmentId { get; set; }
        [Required] [Range(2000, 2100, ErrorMessage = "Year must be a valid value.")]
        public int Year { get; set; }
        [Required]  [Range(1, 12, ErrorMessage = "Month must be a valid value.")]
        public int Month { get; set; }
}