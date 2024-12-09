using Domain.Models;

namespace ApplicationServices.Project;

public interface IProjectService
{
    Task<ProjectModel?> GetProjectByAlias(string alias);
}