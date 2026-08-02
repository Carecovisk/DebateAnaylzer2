using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DebateAnalyzer.Infrastructure.Persistence;

public class DebateAnalyzerDbContextFactory : IDesignTimeDbContextFactory<DebateAnalyzerDbContext>
{
    public DebateAnalyzerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DebateAnalyzerDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=debateanalyzer;Username=postgres;Password=postgres");

        return new DebateAnalyzerDbContext(optionsBuilder.Options);
    }
}
