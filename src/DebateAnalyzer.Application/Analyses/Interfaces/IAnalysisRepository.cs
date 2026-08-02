using DebateAnalyzer.Domain.Entities;

namespace DebateAnalyzer.Application.Analyses.Interfaces;

public interface IAnalysisRepository
{
    Task AddAsync(Analysis analysis, CancellationToken cancellationToken);
    Task<Analysis?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
