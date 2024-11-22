using Infrastructure.Repositories.Usage;

namespace ApplicationServices.Seed;

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
        //for loop for every project
        var projectId = Guid.NewGuid();
        var environmentId = Guid.NewGuid();

        //make project table data
        //make usage table data
        await _usageDocumentRepository.SeedUsageDocument(projectId, environmentId);
        //make credit table
    }
}