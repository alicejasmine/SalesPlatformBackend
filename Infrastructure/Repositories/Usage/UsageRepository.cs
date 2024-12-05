using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Usage;

public class UsageRepository : IUsageRepository
{
    private readonly DbContext _dbContext; 
    private readonly DbSet<Project> DbSetReadOnly;

    public UsageRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        DbSetReadOnly = _dbContext.Set<Project>();
    }

    public async Task<Guid> GetEnvironmentIdByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias cannot be null or empty", nameof(alias));
        }

        var fetchedEntity = await DbSetReadOnly
            .SingleOrDefaultAsync(t => t.Alias == alias);

        if (fetchedEntity == null)
        {
            throw new KeyNotFoundException($"No project found with alias '{alias}'");
        }

        return fetchedEntity.EnvironmentId;
    }
}

