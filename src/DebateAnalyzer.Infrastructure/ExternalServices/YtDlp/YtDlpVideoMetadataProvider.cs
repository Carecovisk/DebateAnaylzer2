using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DebateAnalyzer.Application.Analyses.Exceptions;
using DebateAnalyzer.Application.Analyses.Interfaces;

namespace DebateAnalyzer.Infrastructure.ExternalServices.YtDlp;

public class YtDlpVideoMetadataProvider : IVideoMetadataProvider
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

    public async Task<YouTubeVideoInfo> GetMetadataAsync(string youTubeUrl, CancellationToken cancellationToken)
    {
        using var timeoutCts = new CancellationTokenSource(Timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        var processResult = await RunYtDlpAsync(youTubeUrl, linkedCts.Token);

        var didYtDlpFail = processResult.ExitCode != 0;
        if (didYtDlpFail)
        {
            throw new VideoMetadataUnavailableException(
                youTubeUrl, $"yt-dlp exited with code {processResult.ExitCode}: {processResult.Stderr}");
        }

        return ParseVideoInfo(youTubeUrl, processResult.Stdout);
    }

    private static async Task<YtDlpProcessResult> RunYtDlpAsync(string youTubeUrl, CancellationToken cancellationToken)
    {
        var startInfo = BuildStartInfo(youTubeUrl);

        using var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();

            var stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            return new YtDlpProcessResult(process.ExitCode, stdout, stderr);
        }
        catch (OperationCanceledException)
        {
            TryKill(process);
            throw new VideoMetadataUnavailableException(youTubeUrl, "yt-dlp timed out or was cancelled.");
        }
    }

    private static ProcessStartInfo BuildStartInfo(string youTubeUrl)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add("--dump-json");
        startInfo.ArgumentList.Add("--skip-download");
        startInfo.ArgumentList.Add("--no-warnings");
        startInfo.ArgumentList.Add(youTubeUrl);

        return startInfo;
    }

    private static void TryKill(Process process)
    {
        try
        {
            var hasAlreadyExited = process.HasExited;
            if (!hasAlreadyExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
            // best-effort cleanup; nothing more we can do here
        }
    }

    private static YouTubeVideoInfo ParseVideoInfo(string youTubeUrl, string stdout)
    {
        var dto = DeserializeOutput(youTubeUrl, stdout);

        var isMissingVideoId = string.IsNullOrWhiteSpace(dto.Id);
        if (isMissingVideoId)
        {
            throw new VideoMetadataUnavailableException(youTubeUrl, "yt-dlp returned no video id.");
        }

        var durationSeconds = dto.Duration.HasValue ? (int)dto.Duration.Value : (int?)null;
        var channelName = dto.Channel ?? dto.Uploader;

        return new YouTubeVideoInfo(dto.Id!, dto.Title, durationSeconds, dto.Thumbnail, channelName);
    }

    private static YtDlpOutput DeserializeOutput(string youTubeUrl, string stdout)
    {
        try
        {
            var dto = JsonSerializer.Deserialize<YtDlpOutput>(stdout);
            if (dto is null)
            {
                throw new VideoMetadataUnavailableException(youTubeUrl, "yt-dlp returned empty output.");
            }

            return dto;
        }
        catch (JsonException ex)
        {
            throw new VideoMetadataUnavailableException(youTubeUrl, "yt-dlp returned unparsable output.", ex);
        }
    }

    private record YtDlpProcessResult(int ExitCode, string Stdout, string Stderr);

    private class YtDlpOutput
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("duration")]
        public double? Duration { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("uploader")]
        public string? Uploader { get; set; }
    }
}
