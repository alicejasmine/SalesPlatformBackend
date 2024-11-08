using Domain.Sample;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Sample;

public sealed class SampleRepository : BaseRepository<SampleModel, SampleEntity>, ISampleRepository
{
    public SampleRepository(SalesPlatformDbContext context) : base(context)
    {
    }
    
    public async Task<SampleModel?> GetSampleEntityByIdAsync(Guid id)
    {
        var fetchedEntity = await DbSetReadOnly
            .SingleOrDefaultAsync(t => t.Id == id);

        return fetchedEntity == null ? null : MapEntityToModel(fetchedEntity);
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
}