namespace Domain.Models;

public class ProjectModel : BaseModel
{
    public ProjectModel(Guid id, string displayName, string alias, DateTime created, DateTime modified, PlanModel plan, Guid organizationId)
            : base(id, created, modified)
    {
        EnvironmentId = id;
        DisplayName = displayName;
        Alias = alias;
        Plan = plan;
        OrganizationId = organizationId;
    }

    public Guid EnvironmentId { get; set; }
    public string DisplayName { get; set; }
    public string Alias { get; set; }
    public PlanModel Plan { get; set; }
    public Guid OrganizationId { get; set; }
}