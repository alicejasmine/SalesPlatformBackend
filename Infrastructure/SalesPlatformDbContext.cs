using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SalesPlatformDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities { get; set; }
    
    public SalesPlatformDbContext(DbContextOptions<SalesPlatformDbContext> options) : base(options)
    {
    }
}