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
    public DbSet<CreditHistoryEntity> CreditHistoryEntities { get; set; } 
    
    public SalesPlatformDbContext(DbContextOptions<SalesPlatformDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Organization)
            .WithMany(o => o.Projects)
            .HasForeignKey(p => p.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CreditHistoryEntity>()
            .HasOne(ch => ch.Organization)
            .WithMany(o => o.CreditHistories)
            .HasForeignKey(ch => ch.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}