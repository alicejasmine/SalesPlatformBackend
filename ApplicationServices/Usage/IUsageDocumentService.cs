using Domain.Entities;

namespace ApplicationServices.Usage;

public interface IUsageDocumentService
{
    Task<UsageEntity?> GetUsageEntity(string alias, int month, int year);
    Task<IEnumerable<UsageEntity>?> GetUsageEntitiesForMultipleMonths(Guid environmentId, int month, int year, int monthsToTake);
    Task<(long totalBandwidthInBytes, long totalMediaInBytes)> GetYearOfUsageData(Guid environmentId, int year);
}