using Asp.Versioning;
using Asp.Versioning.Builder;
using DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;
using DebateAnalyzer.Application.Analyses.Queries.GetAnalysisStatus;
using MediatR;

namespace DebateAnalyzer.Api.Analyses;

public static class AnalysesEndpoints
{
    public static WebApplication MapAnalysesEndpoints(this WebApplication app)
    {
        var versionSet = BuildApiVersionSet(app);

        var group = app.MapGroup("/api/v{version:apiVersion}/analyses")
            .WithApiVersionSet(versionSet);

        group.MapPost("/", SubmitAnalysis);
        group.MapGet("/{id:guid}", GetAnalysisStatus);

        return app;
    }

    private static ApiVersionSet BuildApiVersionSet(WebApplication app)
        => app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

    private static async Task<IResult> SubmitAnalysis(
        SubmitAnalysisRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new SubmitAnalysisCommand(request.YouTubeUrl);
        var result = await sender.Send(command, cancellationToken);

        return Results.Created($"/api/v1/analyses/{result.Id}", result);
    }

    private static async Task<IResult> GetAnalysisStatus(
        Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetAnalysisStatusQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return Results.Ok(result);
    }
}
