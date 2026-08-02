using MediatR;

namespace DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;

public record SubmitAnalysisCommand(string YouTubeUrl) : IRequest<SubmitAnalysisResult>;
