using Domain.Models;
using Infrastructure.Repositories.Project;

namespace ApplicationServices.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<ProjectModel?> GetProjectByAlias(string alias)
    {
        return await _projectRepository.GetProjectByAlias(alias);
    }
}