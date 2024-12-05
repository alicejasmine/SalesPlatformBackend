namespace Domain.Entities;
public class ProjectEntity : BaseEntity
{
    public Guid EnvironmentId { get; set; }
    public string DisplayName { get; set; }
    public string Alias { get; set; }
    public PlanEntity Plan { get; set; }
    public Guid OrganizationId { get; set; }
}