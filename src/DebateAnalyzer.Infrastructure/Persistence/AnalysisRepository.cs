using DebateAnalyzer.Application.Analyses.Interfaces;
using DebateAnalyzer.Domain.Entities;

namespace DebateAnalyzer.Infrastructure.Persistence;

public class AnalysisRepository(DebateAnalyzerDbContext dbContext) : IAnalysisRepository
{
    public async Task AddAsync(Analysis analysis, CancellationToken cancellationToken)
    {
        await dbContext.Analyses.AddAsync(analysis, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
