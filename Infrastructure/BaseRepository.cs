using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public abstract class BaseRepository<TModel, TEntity> : IBaseRepository<TModel>
        where TModel : BaseModel
        where TEntity : BaseEntity
{
    protected IQueryable<TEntity> DbSetReadOnly => _dbSet.AsNoTracking();
    protected DbSet<TEntity> DbSet => _dbSet;

    protected SalesPlatformDbContext Context { get; }

    private readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(SalesPlatformDbContext context)
    {
        Context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        var fetchedEntity = await DbSetReadOnly
            .SingleOrDefaultAsync(t => t.Id == id);

        return fetchedEntity == null ? null : MapEntityToModel(fetchedEntity);
    }

    public async Task<TModel> UpsertAsync(TModel model)
    {
        var existingEntity = await DbSetReadOnly
            .SingleOrDefaultAsync(t => t.Id == model.Id);

        if (existingEntity == null)
        {
            return await AddAsync(model);
        }

        var updatedEntity = MapModelToEntity(model);

        Context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        existingEntity.Modified = DateTime.Now;

        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        return MapEntityToModel(existingEntity);
    }

    private async Task<TModel> AddAsync(TModel model)
    {
        var entity = MapModelToEntity(model);

        DbSet.Add(entity);

        await Context.SaveChangesAsync();

        return MapEntityToModel(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.SingleOrDefaultAsync(t => t.Id == id);

        if (entity == null)
        {
            throw new ArgumentException();
        }

        DbSet.Remove(entity);

        await Context.SaveChangesAsync();
    }

    protected abstract TModel MapEntityToModel(TEntity entity);
    protected abstract TEntity MapModelToEntity(TModel model);
}