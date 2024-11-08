using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SalesPlatformDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"");
    }
}