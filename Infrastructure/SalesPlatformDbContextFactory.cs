using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class SalesPlatformDbContextFactory : IDesignTimeDbContextFactory<SalesPlatformDbContext>
    {
        public SalesPlatformDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SalesPlatformDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<SalesPlatformDbContextFactory>()
                .Build();
            var connectionString = Environment.GetEnvironmentVariable("sqlconn")
                       ?? throw new InvalidOperationException("Database connection string not set.");
            optionsBuilder.UseSqlServer(connectionString);

            return new SalesPlatformDbContext(optionsBuilder.Options);
        }
    }
}