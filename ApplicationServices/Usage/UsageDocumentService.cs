using Infrastructure.Repositories.Usage;
using Microsoft.Extensions.Logging;

namespace ApplicationServices;

public class UsageDocumentService: IUsageDocumentService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    private readonly ILogger<UsageDocumentService> _logger;
    
    public UsageDocumentService(IUsageDocumentRepository usageDocumentRepository, ILogger<UsageDocumentService> logger)
    {
        _usageDocumentRepository = usageDocumentRepository;
        _logger = logger;
    }
}