using Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Integration.Tests
{
    public abstract class SqlDbTestFixture : ConfigurationTestFixture
    {
        protected DbContextOptions<SalesPlatformDbContext> DbContextOptions;
        protected SalesPlatformDbContext _DbContext;

        [SetUp]
        public async Task Setup()
        {
            var connectionString = Environment.GetEnvironmentVariable("sqlconn");

            DbContextOptions = new DbContextOptionsBuilder<SalesPlatformDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _DbContext = new SalesPlatformDbContext(DbContextOptions);

            await _DbContext.Database.EnsureCreatedAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            if (_DbContext != null)
            {
                await _DbContext.Database.CloseConnectionAsync();
                _DbContext.Dispose();
            }
        }
    }
}
