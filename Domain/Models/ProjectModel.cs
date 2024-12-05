namespace Domain.Models;

public class ProjectModel : BaseModel
{
    public ProjectModel(Guid id, Guid environmentId, string alias, string displayName, Guid planId, Guid organizationId, DateTime created, DateTime modified)
            : base(id, created, modified)
    {
        EnvironmentId = environmentId;
        DisplayName = displayName;
        Alias = alias;
        PlanId = planId;
        OrganizationId = organizationId;
    }

    public Guid EnvironmentId { get; set; }
    public string Alias { get; set; }
    public string DisplayName { get; set; }
    public Guid PlanId { get; set; }
    public Guid OrganizationId { get; set; }
}