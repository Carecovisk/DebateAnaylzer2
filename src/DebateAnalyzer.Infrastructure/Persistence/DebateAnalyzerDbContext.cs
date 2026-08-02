using DebateAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebateAnalyzer.Infrastructure.Persistence;

public class DebateAnalyzerDbContext(DbContextOptions<DebateAnalyzerDbContext> options)
    : DbContext(options)
{
    public DbSet<Analysis> Analyses => Set<Analysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DebateAnalyzerDbContext).Assembly);
    }
}
