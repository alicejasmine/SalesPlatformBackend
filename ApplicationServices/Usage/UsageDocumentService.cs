using Domain.ValueObject;
using Domain.Entities;
using Infrastructure.Repositories.Usage;
using Microsoft.Extensions.Logging;

namespace ApplicationServices.Usage;

public class UsageDocumentService: IUsageDocumentService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    private readonly ILogger<UsageDocumentService> _logger;
    
    public UsageDocumentService(IUsageDocumentRepository usageDocumentRepository, ILogger<UsageDocumentService> logger)
    {
        _usageDocumentRepository = usageDocumentRepository;
        _logger = logger;
    }

    public async Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year)
    {
        var date = new DateOnly(year, month, 1);
        var documentIdentifier = new DocumentIdentifier(environmentId, date);
        
        return await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
    }

    public async Task<IEnumerable<UsageEntity>> GetUsageEntities(Guid environmentId, int month, int year, int monthsToTake)
    {
        var usageData = new List<UsageEntity>();

        for (var i = 0; i < monthsToTake; i++)
        {
            var date = new DateOnly(year, month, 1).AddMonths(-i);
            var documentIdentifier = new DocumentIdentifier(environmentId, date);

            var usageEntity = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
            if (usageEntity != null)
            {
                usageData.Add(usageEntity);
            }
        }

        return usageData;
    }
}