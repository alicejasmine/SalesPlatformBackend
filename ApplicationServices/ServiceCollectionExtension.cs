using ApplicationServices.Sample;
using Infrastructure;
using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationServices;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISampleService, SampleService>();
        
       return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISampleRepository, SampleRepository>();
       
        return services;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        string connectionString = Environment.GetEnvironmentVariable("sqlconn")
                                  ?? throw new InvalidOperationException("Database connection string not set.");

        services.AddDbContext<SalesPlatformDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}