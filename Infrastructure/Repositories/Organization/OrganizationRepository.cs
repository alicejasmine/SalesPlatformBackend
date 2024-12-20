using Domain.Entities;
using Domain.Models;
using Infrastructure.Repositories.Credit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Organization;

public class OrganizationRepository : BaseRepository<OrganizationModel, OrganizationEntity>, IOrganizationRepository
{
    private readonly ICreditRepository _creditRepository;
    public OrganizationRepository(SalesPlatformDbContext context, ICreditRepository creditRepository) : base(context)
    {
        _creditRepository = creditRepository;
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
            entity.Modified,
            entity.CreditHistories?.Select(MapCreditHistoryEntityToModel).ToList());
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
            model.Modified,
            model.CreditHistory?.Select(MapCreditHistoryModelToEntity).ToList());
    }
    
    private CreditHistoryModel MapCreditHistoryEntityToModel(CreditHistoryEntity entity)
    {
        return new CreditHistoryModel(
            entity.Id,
            entity.Created,
            entity.Modified,
            entity.InvoiceNumber,
            entity.PartnershipCredits,
            entity.CreditsSpend,
            entity.CurrentCredits,
            entity.OrganizationId
        );
    }

    private CreditHistoryEntity MapCreditHistoryModelToEntity(CreditHistoryModel model)
    {
        return new CreditHistoryEntity
        {
            Id = model.Id,
            Created = model.Created,
            Modified = model.Modified,
            InvoiceNumber = model.InvoiceNumber,
            PartnershipCredits = model.PartnershipCredits,
            CreditsSpend = model.CreditsSpend,
            CurrentCredits = model.CurrentCredits,
            OrganizationId = model.OrganizationId
        };
    }
}