using System.ComponentModel.DataAnnotations;

namespace Api.Service.Credits.DTOs;

public class GetCreditsHistoryByOrganizationAliasRequestDto
{
    [Required]
    public string OrganizationAlias { get; set; }
}