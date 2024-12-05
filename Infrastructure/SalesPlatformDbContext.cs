using Domain.Entities;
using Domain.Sample;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SalesPlatformDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities { get; set; }
    public DbSet<ProjectEntity> ProjectEntities { get; set; }
    public DbSet<PlanEntity> PlanEntities { get; set; }
    public DbSet<OrganizationEntity> organizationEntities { get; set; }
    
    public SalesPlatformDbContext(DbContextOptions<SalesPlatformDbContext> options) : base(options)
    {
    }
}