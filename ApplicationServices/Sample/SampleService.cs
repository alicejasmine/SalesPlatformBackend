using Domain.Sample;
using Infrastructure.Repository.Sample;
using System.Collections.Immutable;

namespace ApplicationServices.Sample;

public sealed class SampleService : ISampleService
{
    private readonly ISampleRepository _sampleRepository;

    public SampleService(ISampleRepository SampleRepository)
    {
        _sampleRepository = SampleRepository;
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

    public async Task<IImmutableList<SampleDto>> GetAllSamplesAsync()
    {
        var sampleEntities = await _sampleRepository.GetAllSamplesAsync();

        return sampleEntities.Select(ToFullDto).ToImmutableArray();
    }

    public async Task CreateSampleAsync(SampleDto sampleDto)
    {
        var sampleModel = new SampleModel(
            sampleDto.Id,
            sampleDto.Name,
            sampleDto.Description,
            sampleDto.Price,
            DateTime.Now,
            DateTime.Now
        );

        await _sampleRepository.UpsertAsync(sampleModel);
    }

    public async Task DeleteSampleAsync(Guid id)
    {
        await _sampleRepository.DeleteAsync(id);
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