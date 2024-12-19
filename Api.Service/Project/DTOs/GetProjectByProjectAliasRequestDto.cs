using System.ComponentModel.DataAnnotations;

namespace Api.Service.Project.DTOs;

public class GetProjectByProjectAliasRequestDto
{
    [Required]
    public string Alias { get; set; }
}