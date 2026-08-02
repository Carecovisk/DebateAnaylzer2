using DebateAnalyzer.Domain.Enums;

namespace DebateAnalyzer.Application.Analyses.Queries.GetAnalysisStatus;

public record GetAnalysisStatusResult(Guid Id, AnalysisStatus Status, string? ErrorMessage);
