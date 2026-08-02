using DebateAnalyzer.Application.Analyses.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DebateAnalyzer.Api.Common;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = BuildProblemDetails(exception);
        if (problemDetails is null)
        {
            return false;
        }

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails? BuildProblemDetails(Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            return BuildValidationProblem(validationException);
        }

        if (exception is VideoMetadataUnavailableException metadataException)
        {
            return BuildMetadataUnavailableProblem(metadataException);
        }

        return null;
    }

    private static ProblemDetails BuildValidationProblem(ValidationException exception)
    {
        var errorsByProperty = GroupErrorsByProperty(exception);

        return new ValidationProblemDetails(errorsByProperty)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred."
        };
    }

    private static Dictionary<string, string[]> GroupErrorsByProperty(ValidationException exception)
    {
        var errorsByProperty = new Dictionary<string, List<string>>();

        foreach (var error in exception.Errors)
        {
            if (!errorsByProperty.TryGetValue(error.PropertyName, out var messages))
            {
                messages = [];
                errorsByProperty[error.PropertyName] = messages;
            }

            messages.Add(error.ErrorMessage);
        }

        return errorsByProperty.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
    }

    private static ProblemDetails BuildMetadataUnavailableProblem(VideoMetadataUnavailableException exception)
        => new()
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Video metadata could not be retrieved.",
            Detail = exception.Message
        };
}
