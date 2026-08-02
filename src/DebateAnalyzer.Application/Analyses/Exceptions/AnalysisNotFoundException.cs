namespace DebateAnalyzer.Application.Analyses.Exceptions;

public class AnalysisNotFoundException(Guid id)
    : Exception($"Analysis '{id}' was not found.")
{
    public Guid AnalysisId { get; } = id;
}
