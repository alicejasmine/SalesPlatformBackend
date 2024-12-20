using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Service.Usage.DTOs;

public class GetMonthlyUsageRequestDto
{
        [Required] 
        public string Alias { get; set; }
        [Required] [Range(2000, 2100, ErrorMessage = "Year must be a valid value.")]
        public int Year { get; set; }
        [Required]  [Range(1, 12, ErrorMessage = "Month must be a valid value.")]
        public int Month { get; set; }
}