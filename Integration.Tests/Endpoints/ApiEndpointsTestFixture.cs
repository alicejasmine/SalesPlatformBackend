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

namespace Integration.Tests.Endpoints
{
    public abstract class ApiEndpointsTestFixture : IDisposable
    {
        protected HttpClient Client { get; private set; }
        private WebApplicationFactory<Program> _factory;
        private MsSqlContainer _sqlContainer;

        [OneTimeSetUp]
        public async Task SettingUpSql()
        {
            _sqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithPassword("YourStrongPassword123!")
                .Build();

            await _sqlContainer.StartAsync();
        }

        [SetUp]
        public void SetUp()
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
