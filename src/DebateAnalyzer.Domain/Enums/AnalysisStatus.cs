namespace DebateAnalyzer.Domain.Enums;

public enum AnalysisStatus
{
    Queued,
    Downloading,
    Transcribing,
    Analyzing,
    Completed,
    Failed
}
