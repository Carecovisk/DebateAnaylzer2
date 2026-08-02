using DebateAnalyzer.Application.Analyses.Interfaces;
using DebateAnalyzer.Domain.Entities;
using DebateAnalyzer.Domain.Services;
using DebateAnalyzer.Domain.ValueObjects;
using MediatR;

namespace DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;

public class SubmitAnalysisCommandHandler(
    IVideoMetadataProvider metadataProvider,
    IAnalysisRepository repository)
    : IRequestHandler<SubmitAnalysisCommand, SubmitAnalysisResult>
{
    public async Task<SubmitAnalysisResult> Handle(SubmitAnalysisCommand request, CancellationToken cancellationToken)
    {
        var videoId = ExtractVideoId(request.YouTubeUrl);
        var info = await metadataProvider.GetMetadataAsync(request.YouTubeUrl, cancellationToken);
        var video = BuildVideoMetadata(request.YouTubeUrl, videoId, info);

        var analysis = Analysis.Create(video);
        await repository.AddAsync(analysis, cancellationToken);

        return new SubmitAnalysisResult(analysis.Id, analysis.Status);
    }

    private static string ExtractVideoId(string url)
    {
        // Already validated by SubmitAnalysisCommandValidator; re-parsing here is
        // defense-in-depth in case this handler is ever invoked outside the MediatR pipeline.
        var isValid = YouTubeUrlParser.TryParseVideoId(url, out var id);
        if (!isValid)
        {
            throw new InvalidOperationException("YouTubeUrl failed re-validation in handler.");
        }

        return id;
    }

    private static VideoMetadata BuildVideoMetadata(string url, string videoId, YouTubeVideoInfo info) => new()
    {
        YouTubeUrl = url,
        YouTubeVideoId = videoId,
        Title = info.Title,
        SourceName = info.ChannelName,
        DurationSeconds = info.DurationSeconds,
        ThumbnailUrl = info.ThumbnailUrl
    };
}
