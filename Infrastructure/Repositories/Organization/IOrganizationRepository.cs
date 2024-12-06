using Domain.Models;

namespace Infrastructure.Repositories.Organization;

public interface IOrganizationRepository : IBaseRepository<OrganizationModel>
{
    new Task<OrganizationModel?> GetByIdAsync(Guid id);
}