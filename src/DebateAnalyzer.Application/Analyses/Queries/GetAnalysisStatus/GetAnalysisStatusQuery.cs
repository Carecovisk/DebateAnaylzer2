using MediatR;

namespace DebateAnalyzer.Application.Analyses.Queries.GetAnalysisStatus;

public record GetAnalysisStatusQuery(Guid Id) : IRequest<GetAnalysisStatusResult>;
