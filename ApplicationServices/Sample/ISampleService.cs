using Domain.Sample;
using System.Collections.Immutable;

namespace ApplicationServices.Sample;
public interface ISampleService
{
    Task<IImmutableList<SampleDto>> GetAllSamplesAsync();
    Task<SampleDto?> GetSampleByIdAsync(Guid Id);
    Task CreateSampleAsync(SampleDto sampleModel);
}