using Respawn;

namespace Integration.Tests.Library;

[TestFixture]
public abstract class BaseDatabaseTestFixture
{
    private Respawner _respawner;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _respawner = await Respawner.CreateAsync(DatabaseTestsFixture.ConnectionString);
    }

    [SetUp]
    public async Task Setup()
    {
        await _respawner.ResetAsync(DatabaseTestsFixture.ConnectionString);
        DatabaseTestsFixture.DbContext.ChangeTracker.Clear();
    }
}