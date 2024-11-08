using Domain.Sample;

namespace Infrastructure.Repository.Sample;

public interface ISampleRepository : IBaseRepository<SampleModel>
{
    Task<SampleModel> GetSampleEntityByIdAsync(Guid id);
}