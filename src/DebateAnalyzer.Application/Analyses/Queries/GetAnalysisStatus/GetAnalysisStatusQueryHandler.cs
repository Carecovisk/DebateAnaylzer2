using DebateAnalyzer.Application.Analyses.Exceptions;
using DebateAnalyzer.Application.Analyses.Interfaces;
using MediatR;

namespace DebateAnalyzer.Application.Analyses.Queries.GetAnalysisStatus;

public class GetAnalysisStatusQueryHandler(IAnalysisRepository repository)
    : IRequestHandler<GetAnalysisStatusQuery, GetAnalysisStatusResult>
{
    public async Task<GetAnalysisStatusResult> Handle(GetAnalysisStatusQuery request, CancellationToken cancellationToken)
    {
        var analysis = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AnalysisNotFoundException(request.Id);

        return new GetAnalysisStatusResult(analysis.Id, analysis.Status, analysis.ErrorMessage);
    }
}
