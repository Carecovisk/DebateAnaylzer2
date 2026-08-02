namespace DebateAnalyzer.Domain.ValueObjects;

public class VideoMetadata
{
    public string YouTubeUrl { get; set; } = null!;
    public string YouTubeVideoId { get; set; } = null!;
    public string? Title { get; set; }
    public string? SourceName { get; set; }
    public int? DurationSeconds { get; set; }
    public string? ThumbnailUrl { get; set; }
}
