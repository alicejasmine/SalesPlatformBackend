using Domain.Entities;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Repositories.Project;

namespace Infrastructure.Repositories.Plan;

public class PlanRepository : BaseRepository<PlanModel, PlanEntity>, IPlanRepository
{
    public PlanRepository(SalesPlatformDbContext context) : base(context)
    {
    }

    protected override PlanModel MapEntityToModel(PlanEntity entity)
    {
        return new PlanModel(
            entity.Id,
            entity.Name,
            entity.PriceInDKK,
            entity.Plan,
            entity.Created,
            entity.Modified);
    }

    protected override PlanEntity MapModelToEntity(PlanModel model)
    {
        return new PlanEntity(
            model.Id,
            model.Name,
            model.PriceInDKK,
            model.Plan,
            model.Created,
            model.Modified);
    }
}