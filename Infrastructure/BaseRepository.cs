using Domain.Models;
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
        
        protected abstract TModel MapEntityToModel(TEntity entity);
}