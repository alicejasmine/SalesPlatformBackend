using Domain.Models;

namespace Infrastructure.Data.Sample;

public interface ISampleRepository
{
    Task<SampleModel> GetSampleEntityByIdAsync(Guid id);
}