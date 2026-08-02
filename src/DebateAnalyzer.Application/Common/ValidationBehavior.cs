using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace DebateAnalyzer.Application.Common;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var hasValidators = validators.Any();
        if (!hasValidators)
        {
            return await next(cancellationToken);
        }

        var failures = await ValidateAsync(request, cancellationToken);
        var hasFailures = failures.Count != 0;
        if (hasFailures)
        {
            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }

    private async Task<List<ValidationFailure>> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationTasks = validators.Select(validator => validator.ValidateAsync(context, cancellationToken));
        var results = await Task.WhenAll(validationTasks);

        return CollectFailures(results);
    }

    private static List<ValidationFailure> CollectFailures(ValidationResult[] results)
    {
        var failures = new List<ValidationFailure>();
        foreach (var result in results)
        {
            failures.AddRange(result.Errors);
        }

        return failures;
    }
}
