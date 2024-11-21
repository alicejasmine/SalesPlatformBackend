using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Usage;

public class UsageDocumentRepository  : IUsageDocumentRepository
{
    private readonly ILogger<UsageDocumentRepository> _logger;
    public UsageDocumentRepository(ILogger<UsageDocumentRepository> logger)
    {
        _logger = logger;
    }
}