using Domain.Models;

namespace Infrastructure;

public interface IBaseRepository<TModel> where TModel : BaseModel
{
    Task<TModel?> GetByIdAsync(Guid id);
}