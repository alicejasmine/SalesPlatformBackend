using Domain.Dto;
using Domain.Models;

namespace ApplicationServices.Sample;

public class SampleService : ISampleService
{
    //private readonly ISampleRepository _SampleRepository;

    //public SampleService(ISampleRepository SampleRepository)
    //{
    //    _SampleRepository = SampleRepository;
    //}

    public async Task CreateSampleAsync(SampleDto sampleDto)
    {
        var sampleId = Guid.NewGuid();
        var created = DateTime.Now;
        var modified = DateTime.Now;

        var sampleModel = new SampleDto(
            sampleId,
            sampleDto.Name,
            sampleDto.Description,
            sampleDto.Price,
            created,
            modified);

        //await _sampleRepository.UpsertAsync(sampleModel);
    }

    public Task<IEnumerable<SampleModel>> GetAllSamplesAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<SampleModel?> GetSampleAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}