using System.ComponentModel.DataAnnotations;

namespace Api.Service.Project.DTOs;

public class GetProjectsByOrganizationAliasRequestDto
{
    [Required]
    public string OrganizationAlias { get; set; }
}