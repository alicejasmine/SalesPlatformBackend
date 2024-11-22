using Domain.Entities;

namespace ApplicationServices;

public interface IUsageDocumentService
{
    Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year);
}