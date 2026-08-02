namespace DebateAnalyzer.Application.Analyses.Interfaces;

public interface IVideoMetadataProvider
{
    Task<YouTubeVideoInfo> GetMetadataAsync(string youTubeUrl, CancellationToken cancellationToken);
}

public record YouTubeVideoInfo(
    string VideoId,
    string? Title,
    int? DurationSeconds,
    string? ThumbnailUrl,
    string? ChannelName);
