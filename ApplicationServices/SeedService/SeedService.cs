using Infrastructure.Repositories.Usage;
using Microsoft.Extensions.Logging;

namespace ApplicationServices.SeedService;

public class SeedService: ISeedService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    //private readonly ICreditService _creditRepository;
    //private readonly IProjectService _projectRepository;

    public SeedService(IUsageDocumentRepository usageDocumentRepository)
    {
        _usageDocumentRepository = usageDocumentRepository;
    }

    public async Task SeedDatabasesWithData()
    {
        var numberOfSeedData = 5;

        for (var i = 0; i < numberOfSeedData; i++)
        {
            var projectId = Guid.NewGuid();
            var environmentId = Guid.NewGuid();

            //make project table data
                //take the generated project id, environment id, and project alias
            //make usage table data
            await _usageDocumentRepository.SeedUsageDocument(projectId, environmentId);
            //make credit table
        }
    }
}