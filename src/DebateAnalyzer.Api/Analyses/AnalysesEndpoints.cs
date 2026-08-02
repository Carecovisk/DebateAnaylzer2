using Asp.Versioning;
using Asp.Versioning.Builder;
using DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;
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
}
