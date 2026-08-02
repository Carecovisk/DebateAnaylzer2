using DebateAnalyzer.Domain.Services;
using FluentValidation;

namespace DebateAnalyzer.Application.Analyses.Commands.SubmitAnalysis;

public class SubmitAnalysisCommandValidator : AbstractValidator<SubmitAnalysisCommand>
{
    public SubmitAnalysisCommandValidator()
    {
        RuleFor(c => c.YouTubeUrl)
            .NotEmpty()
            .Must(BeAValidYouTubeUrl)
            .WithMessage("YouTubeUrl must be a valid youtube.com or youtu.be video URL.");
    }

    private static bool BeAValidYouTubeUrl(string url)
        => YouTubeUrlParser.TryParseVideoId(url, out _);
}
