using ApplicationServices.Sample;
using ApplicationServices.Seed;
using Infrastructure;
using Infrastructure.CosmosDb;
using Infrastructure.Repositories.Usage;
using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationServices;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISampleService, SampleService>();
        services.AddScoped<IUsageDocumentService, UsageDocumentService>();
        services.AddScoped<ISeedService, SeedService>();
        
       return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISampleRepository, SampleRepository>();
        services.AddScoped<IUsageDocumentRepository, UsageDocumentRepository>();
       
        return services;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var connectionString =  Environment.GetEnvironmentVariable("sqlconn") 
            ?? throw new InvalidOperationException("Database connection string not set.");

        services.AddDbContext<SalesPlatformDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
    
    public static IServiceCollection AddCosmosDb(this IServiceCollection services)
    {
        services.ConfigureCosmosDbContainer();
        return services;
    }
}