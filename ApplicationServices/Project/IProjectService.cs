using Domain.Models;

namespace ApplicationServices.Project;

public interface IProjectService
{
    Task<ProjectModel?> GetProjectByProjectAlias(string alias);
    Task<List<ProjectModel>> GetProjectsByOrganizationAlias(string dtoOrganizationAlias);
    Task<List<ProjectModel>> GetAllProjects();
}