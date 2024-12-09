namespace Api.Service.Project.DTOs;

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Alias { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid PlanId { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}