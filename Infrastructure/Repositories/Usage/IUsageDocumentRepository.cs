using Domain.ValueObject;
using Domain.Entities;

namespace Infrastructure.Repositories.Usage;

public interface IUsageDocumentRepository
{
    Task<UsageEntity?> GetUsageEntity(DocumentIdentifier documentIdentifier);
}