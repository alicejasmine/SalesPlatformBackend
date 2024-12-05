using Domain.Entities;
using Domain.ValueObject;


namespace Infrastructure.Repositories.Usage;

public interface IUsageDocumentRepository
{
    Task<UsageEntity?> GetUsageEntity(DocumentIdentifier documentIdentifier);
    Task SeedUsageDocument(Guid projectId, Guid environmentId);
}