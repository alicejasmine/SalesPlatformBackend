using Domain.Entities;

namespace ApplicationServices.Usage;

public interface IUsageDocumentService
{
    Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year);
    Task<IEnumerable<UsageEntity>?> GetUsageEntities(Guid environmentId, int month, int year, int monthsToTake);
}