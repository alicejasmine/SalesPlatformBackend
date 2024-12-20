using Domain.Entities;
using Domain.Models;
using Domain.Sample;
using Infrastructure.Repositories.Credit;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Project;
using Infrastructure.Repositories.Usage;
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

    public async Task StoreSample(SampleModel sample)
    {
        var repo = _services.GetRequiredService<ISampleRepository>();
        await repo.UpsertAsync(sample);
    }

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

    public async Task StoreUsage(UsageEntity usage)
    {
        var repo = _services.GetRequiredService<IUsageDocumentRepository>();
        await repo.CreateUsageDocument(usage);
    }

    public async Task StoreCreditHistory(List<CreditHistoryModel> creditHistory)
    {
        var repo = _services.GetRequiredService<ICreditRepository>();
        await repo.UpsertAsync(creditHistory[0]);
        await repo.UpsertAsync(creditHistory[1]);
    }
}

