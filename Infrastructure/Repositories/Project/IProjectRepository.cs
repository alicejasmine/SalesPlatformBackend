using Domain.Models;

namespace Infrastructure.Repositories.Project;

public interface IProjectRepository : IBaseRepository<ProjectModel>
{
    Task<ProjectModel?> GetProjectByAlias(string alias);

    Task<Guid> GetEnvironmentIdByAlias(string alias);
    Task<List<ProjectModel>> GetAllProjects();
}