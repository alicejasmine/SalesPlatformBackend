using Domain.Sample;

namespace Infrastructure.Data.Sample;

public interface ISampleRepository
{
    Task<SampleModel> GetSampleEntityByIdAsync(Guid id);
}