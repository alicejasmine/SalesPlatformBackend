using Domain.Entities;
using Domain.Models;
using Domain.ValueObject;
using Infrastructure.Repositories.Project;
using Infrastructure.Repositories.Usage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApplicationServices.Usage;

public class UsageDocumentService : IUsageDocumentService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    private readonly IProjectRepository _projectRepository;

    public UsageDocumentService(IUsageDocumentRepository usageDocumentRepository, IProjectRepository projectRepository)

    {
        _usageDocumentRepository = usageDocumentRepository;
        _projectRepository = projectRepository;
    }

    public async Task<UsageEntity?> GetUsageEntity(string alias, int month, int year)
    {
        var environmentId = await _projectRepository.GetEnvironmentIdByProjectAlias(alias);
        
        var date = new DateOnly(year, month, 1);
        var documentIdentifier = new DocumentIdentifier(environmentId, date);

        return await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
    }

    public async Task<IEnumerable<UsageEntity>?> GetUsageEntitiesForMultipleMonths(string alias, int month, int year, int monthsToTake)
    {
        var usageData = new List<UsageEntity>();
        var environmentId = await _projectRepository.GetEnvironmentIdByProjectAlias(alias);
        
        for (var i = 0; i < monthsToTake; i++)
        {
            var date = new DateOnly(year, month, 1).AddMonths(-i);
            var documentIdentifier = new DocumentIdentifier(environmentId, date);

            var usageEntity = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
            if (usageEntity != null)
            {
                usageData.Add(usageEntity);
            }
        }

        return usageData;
    }

    public async Task<(long totalBandwidthInBytes, long totalMediaInBytes)> GetYearOfUsageData(string alias, int month, int year)
    {
        var usageData = new List<UsageEntity>();
        var environmentId = await _projectRepository.GetEnvironmentIdByAlias(alias);

        for (var i = 0; i < 12; i++)
        {
            var date = new DateOnly(year, month, 1).AddMonths(-i);
            var documentIdentifier = new DocumentIdentifier(environmentId, date);

            var usageEntity = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
            if (usageEntity != null)
            {
                usageData.Add(usageEntity);
            }
        }

        var totalBandwidthInBytes = usageData.Sum(u => u.TotalMonthlyBandwidth);
        var totalMediaInBytes = usageData.Sum(u => u.TotalMonthlyMedia);

        return (totalBandwidthInBytes, totalMediaInBytes);
    }
}