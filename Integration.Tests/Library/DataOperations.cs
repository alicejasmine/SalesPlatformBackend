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

    public async Task StoreSample(SampleModel sample)
    {
        var repo = _services.GetRequiredService<ISampleRepository>();
        await repo.UpsertAsync(sample);
    }
}