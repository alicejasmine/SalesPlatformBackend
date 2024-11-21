using Domain.Sample;
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
}
