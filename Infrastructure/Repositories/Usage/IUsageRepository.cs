namespace Infrastructure.Repositories.Usage;

public interface IUsageRepository
{ 
    Task<Guid> GetEnvironmentIdByAlias(string alias);
}