using Microsoft.Extensions.Logging;

namespace ApplicationServices;

public class UsageDocumentService: IUsageDocumentService
{
    private readonly ILogger<UsageDocumentService> _logger;
    
    public UsageDocumentService(ILogger<UsageDocumentService> logger)
    {
        _logger = logger;
    }
}