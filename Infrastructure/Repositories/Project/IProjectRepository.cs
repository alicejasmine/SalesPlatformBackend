using Domain.Models;

namespace Infrastructure.Repositories.Project;

public interface IProjectRepository : IBaseRepository<ProjectModel>
{
    Task<ProjectModel?> GetProjectByProjectAlias(string projectAlias);

    Task<Guid> GetEnvironmentIdByProjectAlias(string projectAlias);
    Task<List<ProjectModel>> GetAllProjects();
    Task<List<ProjectModel>> GetProjectsByOrganizationAlias(string organizationAlias);
}