using Domain.Models;

namespace ApplicationServices.Credit;

public interface ICreditService
{
    Task<List<CreditHistoryModel>> GetCreditHistoryByOrganizationAlias(string organizationAlias);
}