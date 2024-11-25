namespace Infrastructure.Repositories.Usage;

public interface IUsageDocumentRepository
{
    Task SeedUsageDocument(Guid projectId, Guid environmentId);
}