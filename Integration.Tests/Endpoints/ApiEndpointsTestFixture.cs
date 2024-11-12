using ApplicationServices.Sample;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Testcontainers.MsSql;
using Infrastructure.Repository.Sample;

namespace Integration.Tests.Endpoints
{
    public abstract class ApiEndpointsTestFixture : IDisposable
    {
        protected HttpClient Client { get; private set; }
        private WebApplicationFactory<Program> _factory;
        private MsSqlContainer _sqlContainer;
        private SalesPlatformDbContext _salesPlatformDbContext;

        [OneTimeSetUp]
        public async Task SettingUpSql()
        {
            _sqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithPassword("YourStrongPassword123!")
                .Build();

            await _sqlContainer.StartAsync();

            var connectionString = _sqlContainer.GetConnectionString();

            var options = new DbContextOptionsBuilder<SalesPlatformDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _salesPlatformDbContext = new SalesPlatformDbContext(options);

            await _salesPlatformDbContext.Database.EnsureCreatedAsync();
        }

        [SetUp]
        public async Task SetUp()
        {
            var connectionString = _sqlContainer.GetConnectionString();

            Environment.SetEnvironmentVariable("sqlconn", connectionString);

            _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<SalesPlatformDbContext>>();
                    services.AddDbContext<SalesPlatformDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    services.RemoveAll<ISampleService>();
                    var mockSampleService = new Mock<ISampleService>();
                    services.AddSingleton(mockSampleService.Object);
                });

                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
            });

            Client = _factory.CreateClient();

            await InsertTestDataAsync();
        }

        private async Task InsertTestDataAsync()
        {
            if (!_salesPlatformDbContext.SampleEntities.Any())
            {
                _salesPlatformDbContext.SampleEntities.Add(new SampleEntity(
                    Guid.NewGuid(),
                    "Test sample",
                    "Description of Test sample",
                    100,
                    DateTime.Now,
                    DateTime.Now
                ));
                await _salesPlatformDbContext.SaveChangesAsync();
            }
        }

        [TearDown]
        public async Task CleanUp()
        {
            var products = _salesPlatformDbContext.SampleEntities.ToList();
            _salesPlatformDbContext.SampleEntities.RemoveRange(products);
            await _salesPlatformDbContext.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task DisposeAsync()
        {
            Client?.Dispose();
            _factory?.Dispose();

            if (_sqlContainer is not null)
            {
                await _sqlContainer.StopAsync();
                await _sqlContainer.DisposeAsync();
            }
        }

        protected T GetService<T>() where T : notnull
        {
            return _factory.Services.GetRequiredService<T>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
