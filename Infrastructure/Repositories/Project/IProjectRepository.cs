using Domain.Models;

namespace Infrastructure.Repositories.Project;

public interface IProjectRepository : IBaseRepository<ProjectModel>
{
    Task<Guid> GetEnvironmentIdByAlias(string alias);
}