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
    public async Task<ProjectModel?> GetProjectByAlias(string projectAlias)
    {
        return await _projectRepository.GetProjectByAlias(projectAlias);
    }

    public async Task<List<ProjectModel>> GetProjectsByOrganizationAlias(string organizationAlias)
    {
        return await _projectRepository.GetProjectsByOrganizationAlias(organizationAlias);
    }

    public async Task<List<ProjectModel>> GetAllProjects()
    {
        return await _projectRepository.GetAllProjects();
    }
}