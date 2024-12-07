using Api.Service.Project.DTOs;
using Domain.Models;

namespace Api.Service.Project;

public static class ProjectMapper
{
    public static ProjectResponse MapModelToResponse(ProjectModel project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            DisplayName = project.DisplayName,
            Alias = project.Alias,
            EnvironmentId = project.EnvironmentId,
            PlanId = project.PlanId,
            OrganizationId = project.OrganizationId,
            Created = project.Created,
            Modified = project.Modified
        };
    }
}
