using Domain.Entities;
using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Project;

public class ProjectRepository : BaseRepository<ProjectModel, ProjectEntity>, IProjectRepository
{
    public ProjectRepository(SalesPlatformDbContext context) : base(context)
    {
    }
    
    public async Task<ProjectModel?> GetProjectByAlias(string alias)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("Alias cannot be null or empty", nameof(alias));
            }

            var projectEntity = await Context.Set<ProjectEntity>()
                .FirstOrDefaultAsync(p => p.Alias == alias);

            if (projectEntity == null)
            {
                return null; 
            }

            return MapEntityToModel(projectEntity);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Invalid argument provided: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving the project by alias.", ex);
        }
    }

    public async Task<Guid> GetEnvironmentIdByProjectAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias cannot be null or empty", nameof(alias));
        }

        var fetchedEntity = await DbSetReadOnly
            .SingleOrDefaultAsync(t => t.Alias == alias);

        if (fetchedEntity == null)
        {
            throw new KeyNotFoundException($"No project found with alias '{alias}'");
        }

        return fetchedEntity.EnvironmentId;
    }

    public async Task<List<ProjectModel>> GetAllProjects()
    {
        try
        { 
            var projectEntities = await DbSetReadOnly.ToListAsync();
            
            if (!projectEntities.Any())
            {
                return new List<ProjectModel>();
            }

            return projectEntities.Select(MapEntityToModel).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving the projects.", ex);
        }
    }
    
    protected override ProjectModel MapEntityToModel(ProjectEntity entity)
    {
        return new ProjectModel(
            entity.Id,
            entity.EnvironmentId,
            entity.Alias,
            entity.DisplayName,
            entity.PlanId,
            entity.OrganizationId,
            entity.Created,
            entity.Modified);
    }

    protected override ProjectEntity MapModelToEntity(ProjectModel model)
    {
        return new ProjectEntity(
            model.Id,
            model.EnvironmentId,
            model.DisplayName,
            model.Alias,
            model.PlanId,
            model.OrganizationId,
            model.Created,
            model.Modified
        );
    }
}