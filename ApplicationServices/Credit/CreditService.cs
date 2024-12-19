using Domain.Models;
using Infrastructure.Repositories.Credit;

namespace ApplicationServices.Credit;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    public CreditService(ICreditRepository creditRepository)
    {
        _creditRepository = creditRepository;
    }
    public async Task<List<CreditHistoryModel>> GetCreditHistoryByOrganizationAlias(string organizationAlias)
    {
        return await _creditRepository.GetCreditHistoryByOrganizationAlias(organizationAlias);
    }
}

