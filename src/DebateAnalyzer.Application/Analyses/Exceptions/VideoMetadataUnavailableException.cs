namespace DebateAnalyzer.Application.Analyses.Exceptions;

public class VideoMetadataUnavailableException(string youTubeUrl, string reason, Exception? innerException = null)
    : Exception($"Could not retrieve metadata for '{youTubeUrl}': {reason}", innerException)
{
    public string YouTubeUrl { get; } = youTubeUrl;
}
