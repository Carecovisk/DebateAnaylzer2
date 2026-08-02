using DebateAnalyzer.Domain.Enums;
using DebateAnalyzer.Domain.ValueObjects;

namespace DebateAnalyzer.Domain.Entities;

public class Analysis
{
    public Guid Id { get; set; }
    public VideoMetadata Video { get; set; } = null!;
    public AnalysisStatus Status { get; set; } = AnalysisStatus.Queued;
    public string? ErrorMessage { get; set; }
    public int? TranscriptionProgressSeconds { get; set; }
    public string? AudioBlobUrl { get; set; }
    public string? SummaryText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<Speaker> Speakers { get; set; } = new();
    public List<TranscriptSegment> TranscriptSegments { get; set; } = new();
    public List<Fallacy> Fallacies { get; set; } = new();
    public List<Claim> Claims { get; set; } = new();
    public List<KeyMoment> KeyMoments { get; set; } = new();
}
