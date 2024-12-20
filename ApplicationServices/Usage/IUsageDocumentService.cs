using Domain.Entities;

namespace ApplicationServices.Usage;

public interface IUsageDocumentService
{
    Task<UsageEntity?> GetUsageEntity(string alias, int month, int year);
    Task<IEnumerable<UsageEntity>?> GetUsageEntitiesForMultipleMonths(string alias, int month, int year, int monthsToTake);
    Task<(long totalBandwidthInBytes, long totalMediaInBytes)> GetYearOfUsageData(string alias, int month, int year);
}