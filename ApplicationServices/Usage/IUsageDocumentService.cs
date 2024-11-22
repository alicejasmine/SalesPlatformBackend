using Domain.Entities;

namespace ApplicationServices.Usage;

public interface IUsageDocumentService
{
    Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year);
}