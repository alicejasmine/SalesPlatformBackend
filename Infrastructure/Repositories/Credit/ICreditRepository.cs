using Domain.Models;

namespace Infrastructure.Repositories.Credit;

public interface ICreditRepository
{
    Task UpsertAsync(CreditHistoryModel creditHistory);
    Task<List<CreditHistoryModel>> GetCreditHistoryByOrganizationAlias(string organizationAlias);
}