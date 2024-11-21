using Domain.Sample;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Infrastructure.Repository.Sample;

public sealed class SampleRepository : BaseRepository<SampleModel, SampleEntity>, ISampleRepository
{
    public SampleRepository(SalesPlatformDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<SampleModel>> GetAllSamplesAsync()
    {
        var entities = await DbSetReadOnly.ToListAsync();

        return entities.Select(MapEntityToModel).ToImmutableArray();
    }

    protected override SampleModel MapEntityToModel(SampleEntity entity)
    {
        return new SampleModel(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.Created,
            entity.Modified);
    }

    protected override SampleEntity MapModelToEntity(SampleModel model)
    {
        return new SampleEntity(
            model.Id,
            model.Name,
            model.Description,
            model.Price,
            model.Created,
            model.Modified
        );
    }
}