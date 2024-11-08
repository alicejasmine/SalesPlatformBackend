using Domain.Dto;
using Domain.Models;

namespace ApplicationServices.Sample;
public interface ISampleService
{
    Task<IEnumerable<SampleDto>> GetAllSamplesAsync(Guid Id);
    Task<SampleDto?> GetSampleByIdAsync(Guid Id);
    Task CreateSampleAsync(SampleDto sampleModel);
}