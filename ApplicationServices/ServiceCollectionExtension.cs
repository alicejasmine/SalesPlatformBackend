using ApplicationServices.Credit;
using ApplicationServices.Project;
using ApplicationServices.Sample;
using ApplicationServices.Usage;
using ApplicationServices.Seed;
using Infrastructure;
using Infrastructure.CosmosDb;
using Infrastructure.Repositories.Credit;
using Infrastructure.Repositories.Usage;
using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories.Project;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Plan;

namespace ApplicationServices;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISampleService, SampleService>();
        services.AddScoped<IUsageDocumentService, UsageDocumentService>();
        services.AddScoped<ISeedService, SeedService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICreditService, CreditService>();
        
       return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISampleRepository, SampleRepository>();
        services.AddScoped<IUsageDocumentRepository, UsageDocumentRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<ICreditRepository, CreditRepository>();
        return services;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SalesPlatformDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureCosmosDbContainer(configuration);
        return services;
    }
}