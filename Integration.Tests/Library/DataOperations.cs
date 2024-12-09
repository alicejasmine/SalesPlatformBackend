using Domain.Models;
using Domain.Sample;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Project;
using Infrastructure.Repository.Sample;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Tests.Library;
public sealed class DataOperations
{
    private readonly IServiceProvider _services;

    public DataOperations(IServiceProvider serviceProvider)
    {
        _services = serviceProvider;
    }

    public async Task StoreUser(SampleModel sample)
    {
        var repo = _services.GetRequiredService<ISampleRepository>();
        await repo.UpsertAsync(sample);
    }

    //public async Task StoreOperation(UsageModel operation)
    //{
    //    var repo = _services.GetRequiredService<IUsageRepository>();
    //    await repo.UpsertAsync(operation);
    //}
    public async Task StoreProject(ProjectModel project)
    {
        var repo = _services.GetRequiredService<IProjectRepository>();
        await repo.UpsertAsync(project);
    }
    
    public async Task StoreOrganization(OrganizationModel organization)
    {
        var repo = _services.GetRequiredService<IOrganizationRepository>();
        await repo.UpsertAsync(organization);
    }
}
