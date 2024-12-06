using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class ProjectEntity : BaseEntity
{
    [Required]
    public Guid EnvironmentId { get; set; }

    [Required]
    [MaxLength(50)]
    public string DisplayName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Alias { get; set; }

    [Required]
    public Guid PlanId { get; set; }
    
    [Required]
    public Guid OrganizationId { get; set; }

    public OrganizationEntity Organization { get; set; }

    public ProjectEntity(Guid id, Guid environmentId, string displayName, string alias, Guid planId, Guid organizationId, DateTime created, DateTime modified) : base(id, created, modified)
    {
        EnvironmentId = environmentId;
        DisplayName = displayName;
        Alias = alias;
        PlanId = planId;
        OrganizationId = organizationId;
    }
}