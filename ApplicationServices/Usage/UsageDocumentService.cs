using Domain.Entities;
using Domain.ValueObject;
using Infrastructure.Repositories.Usage;
using Microsoft.Extensions.Logging;

namespace ApplicationServices.Usage;

public class UsageDocumentService: IUsageDocumentService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    private readonly ILogger<UsageDocumentService> _logger;
    
    public UsageDocumentService(IUsageDocumentRepository usageDocumentRepository, ILogger<UsageDocumentService> logger)
    {
        _usageDocumentRepository = usageDocumentRepository;
        _logger = logger;
    }

    public async Task<UsageEntity?> GetUsageEntity(Guid environmentId, int month, int year)
    
    {
        var date = new DateOnly(year, month, 1);
        var documentIdentifier = new DocumentIdentifier(environmentId, date);
        
        return await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
    }
}