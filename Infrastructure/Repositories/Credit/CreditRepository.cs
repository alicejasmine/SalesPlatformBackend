using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Credit;
public class CreditRepository : BaseRepository<CreditHistoryModel, CreditHistoryEntity>, ICreditRepository
{
    public CreditRepository(SalesPlatformDbContext context) : base(context)
    {
    }

    public async Task UpsertAsync(CreditHistoryModel creditHistory)
    {
        var existingCreditHistory = await DbSet
            .FirstOrDefaultAsync(ch => ch.Id == creditHistory.Id);

        if (existingCreditHistory != null)
        {
            existingCreditHistory.InvoiceNumber = creditHistory.InvoiceNumber;
            existingCreditHistory.PartnershipCredits = creditHistory.PartnershipCredits;
            existingCreditHistory.CreditsSpend = creditHistory.CreditsSpend;
            existingCreditHistory.CurrentCredits = creditHistory.CurrentCredits;
            existingCreditHistory.CreditStart = creditHistory.CreditStart;
            existingCreditHistory.CreditEnd = creditHistory.CreditEnd;
            existingCreditHistory.OrganizationId = creditHistory.OrganizationId;
            existingCreditHistory.Modified = DateTime.UtcNow; // Update modified date

            await Context.SaveChangesAsync();
        }
        else
        {
            var entity = MapModelToEntity(creditHistory);
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }
    }


    protected override CreditHistoryModel MapEntityToModel(CreditHistoryEntity entity)
    {
        return new CreditHistoryModel(
            entity.Id,
            entity.Created,
            entity.Modified,
            entity.InvoiceNumber,
            entity.PartnershipCredits,
            entity.CreditsSpend,
            entity.CurrentCredits,
            entity.CreditStart,
            entity.CreditEnd,
            entity.OrganizationId
        );
    }

    protected override CreditHistoryEntity MapModelToEntity(CreditHistoryModel model)
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
            CreditStart = model.CreditStart,
            CreditEnd = model.CreditEnd,
            OrganizationId = model.OrganizationId
        };
    }
}