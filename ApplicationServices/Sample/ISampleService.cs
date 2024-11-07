using Domain.Dto;
using Domain.Models;

namespace ApplicationServices.Sample;
public interface ISampleService
{
    Task<IEnumerable<SampleModel>> GetAllSamplesAsync(Guid Id);
    Task<SampleModel?> GetSampleAsync(Guid Id);
    Task CreateSampleAsync(SampleDto sampleModel);
}