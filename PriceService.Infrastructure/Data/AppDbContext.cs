using Microsoft.EntityFrameworkCore;
using PriceService.Domain.Entities;

namespace PriceService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<PriceRecord> Prices => Set<PriceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceRecord>()
            .HasIndex(p => p.TimestampHour)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
