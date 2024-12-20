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
            existingCreditHistory.OrganizationId = creditHistory.OrganizationId;
            existingCreditHistory.Modified = DateTime.UtcNow; 

            await Context.SaveChangesAsync();
        }
        else
        {
            var entity = MapModelToEntity(creditHistory);
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }
    }

    public async Task<Guid> GetOrganizationIdByAlias(string organizationAlias)
    {
        var organization = await Context.organizationEntities
            .FirstOrDefaultAsync(o => o.Alias == organizationAlias);

        if (organization == null)
        {
            throw new KeyNotFoundException($"No organization found with alias '{organizationAlias}'.");
        }

        return organization.Id;
    }

    public async Task<List<CreditHistoryModel>> GetCreditHistoryByOrganizationAlias(string organizationAlias)
    {
        if (string.IsNullOrWhiteSpace(organizationAlias))
        {
            throw new ArgumentException("OrganizationAlias cannot be null or empty");
        }
        try
        {
            var organizationId = await GetOrganizationIdByAlias(organizationAlias);

            var creditHistoryEntities = await DbSet
                .Where(ch => ch.OrganizationId == organizationId)
                .OrderByDescending(ch => ch.Created) 
                .ToListAsync();

            if (!creditHistoryEntities.Any())
            {
                return new List<CreditHistoryModel>();
            }

            return creditHistoryEntities.Select(MapEntityToModel).ToList();
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving the credits history.", ex);
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
            OrganizationId = model.OrganizationId
        };
    }
}