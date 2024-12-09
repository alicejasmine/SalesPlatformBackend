using Domain.Entities;
using Domain.Enum;
using Domain.Models;
using Domain.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Project;

public class ProjectRepository : BaseRepository<ProjectModel, ProjectEntity>, IProjectRepository
{
    public ProjectRepository(SalesPlatformDbContext context) : base(context)
    {
    }

    protected override ProjectModel MapEntityToModel(ProjectEntity entity)
    {
        return new ProjectModel(
            entity.Id,
            entity.EnvironmentId,
            entity.DisplayName,
            entity.Alias,
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
    
    public async Task<Guid> GetEnvironmentIdByAlias(string alias)
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
}