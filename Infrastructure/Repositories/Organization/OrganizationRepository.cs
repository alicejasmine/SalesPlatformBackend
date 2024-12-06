using Domain.Entities;
using Domain.Enum;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Organization;

public class OrganizationRepository : BaseRepository<OrganizationModel, OrganizationEntity>, IOrganizationRepository
{
    public OrganizationRepository(SalesPlatformDbContext context) : base(context)
    {
    }

    public override async Task<OrganizationModel?> GetByIdAsync(Guid id)
    {
        var fetchedEntity = await DbSetReadOnly
            .Include(p => p.Projects)
            .SingleOrDefaultAsync(t => t.Id == id);

        return fetchedEntity == null ? null : MapEntityToModel(fetchedEntity);
    }

    protected override OrganizationModel MapEntityToModel(OrganizationEntity entity)
    {
        return new OrganizationModel(
            entity.Id,
            entity.Alias,
            entity.DisplayName,
            entity.TotalCredits,
            entity.Partnership,
            entity.Created,
            entity.Modified);
    }

    protected override OrganizationEntity MapModelToEntity(OrganizationModel model)
    {
        return new OrganizationEntity(
            model.Id,
            model.Alias,
            model.DisplayName,
            model.TotalCredits,
            model.Partnership,
            model.Created,
            model.Modified);
    }
}