using Domain.Sample;
using Infrastructure.Repository.Sample;

namespace ApplicationServices.Sample;

public sealed class SampleService : ISampleService
{
    private readonly ISampleRepository _sampleRepository;

    public SampleService(ISampleRepository SampleRepository)
    {
        _sampleRepository = SampleRepository;
    }

    public async Task CreateSampleAsync(SampleDto sampleDto)
    {
        var sampleId = Guid.NewGuid();
        var created = DateTime.Now;
        var modified = DateTime.Now;

        var sampleModel = new SampleModel(
            sampleId,
            sampleDto.Name,
            sampleDto.Description,
            sampleDto.Price,
            created,
            modified);

        await _sampleRepository.UpsertAsync(sampleModel);
    }

    public Task<IEnumerable<SampleDto>> GetAllSamplesAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task<SampleDto?> GetSampleByIdAsync(Guid id)
    {
        var sampleDto = await _sampleRepository.GetSampleEntityByIdAsync(id);

        if (sampleDto == null)
        {
            return null;
        }
        return ToFullDto(sampleDto);
    }

    private static SampleDto ToFullDto(SampleModel user)
    {
        return new SampleDto(
            user.Id,
            user.Name,
            user.Description,
            user.Price,
            user.Created,
            user.Modified
        );
    }
}