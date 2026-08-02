using DebateAnalyzer.Application.Analyses.Interfaces;
using DebateAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebateAnalyzer.Infrastructure.Persistence;

public class AnalysisRepository(DebateAnalyzerDbContext dbContext) : IAnalysisRepository
{
    public async Task AddAsync(Analysis analysis, CancellationToken cancellationToken)
    {
        await dbContext.Analyses.AddAsync(analysis, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Analysis?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Analyses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
}
