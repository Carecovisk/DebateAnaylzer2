using DebateAnalyzer.Domain.Enums;

namespace DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;

public record SubmitAnalysisResult(Guid Id, AnalysisStatus Status);
