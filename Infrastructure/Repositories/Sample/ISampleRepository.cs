using Domain.Sample;
using System.Collections.Immutable;

namespace Infrastructure.Repository.Sample;

public interface ISampleRepository : IBaseRepository<SampleModel>
{
    Task<IEnumerable<SampleModel>> GetAllSamplesAsync();
}