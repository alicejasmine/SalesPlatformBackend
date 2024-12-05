using Domain.Entities;
using Domain.ValueObject;
using Infrastructure.Repositories.Usage;

namespace ApplicationServices.Usage;

public class UsageDocumentService : IUsageDocumentService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;

    public UsageDocumentService(IUsageDocumentRepository usageDocumentRepository)
    {
        _usageDocumentRepository = usageDocumentRepository;
    }

    public async Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year)
    {
        var date = new DateOnly(year, month, 1);
        var documentIdentifier = new DocumentIdentifier(environmentId, date);

        return await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
    }

    public async Task<IEnumerable<UsageEntity>> GetUsageEntitiesForMultipleMonths(Guid environmentId, int month, int year, int monthsToTake)
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

    public async Task<(long totalBandwidthInBytes, long totalMediaInBytes)> GetYearOfUsageData(Guid environmentId, int year)
    {
        var usageData = new List<UsageEntity>();
        var currentDate = DateTime.UtcNow;

        for (var i = 0; i < 12; i++)
        {
            var date = new DateOnly(year, currentDate.Month, 1).AddMonths(-i);
            var documentIdentifier = new DocumentIdentifier(environmentId, date);

            var usageEntity = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
            if (usageEntity != null)
            {
                usageData.Add(usageEntity);
            }
        }

        var totalBandwidthInBytes = usageData.Sum(u => u.TotalMonthlyBandwidth);
        var totalMediaInBytes = usageData.Sum(u => u.TotalMonthlyMedia);

        return (totalBandwidthInBytes, totalMediaInBytes);
    }
}