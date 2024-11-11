using Domain.Sample;
using System.Collections.Immutable;

namespace Infrastructure.Repository.Sample;

public interface ISampleRepository : IBaseRepository<SampleModel>
{
    Task<SampleModel> GetSampleEntityByIdAsync(Guid id);
    Task<IEnumerable<SampleModel>> GetAllSamplesAsync();
}