using Demo.Products.Api.Extensions;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static LanguageExt.Prelude;

namespace Demo.Products.Api.Core.Filters;

public class ApiValidationFilter : IAsyncActionFilter
{
    private readonly ICustomValidatorFactory _validatorFactory;

    public ApiValidationFilter(ICustomValidatorFactory validatorFactory) =>
        _validatorFactory = validatorFactory;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        if (!context.ActionArguments.Any())
        {
            await next();
            return;
        }

        var validationFailures = new List<ValidationFailure>();

        foreach (var actionArgument in context.ActionArguments)
        {
            var validationErrors = await GetValidationErrorsAsync(actionArgument.Value);
            validationFailures.AddRange(validationErrors);
        }

        if (!validationFailures.Any())
        {
            await next();
            return;
        }

        context.Result = new BadRequestObjectResult(validationFailures.ToProblemDetails());
    }

    private async Task<IEnumerable<ValidationFailure>> GetValidationErrorsAsync(object value)
    {
        var operation = await (
            from _ in GetNonNullValueOrError(value, Error.New(500, "instance is null"))
            from vr in GetValidationResult(value)
            select vr
        ).Run();

        return operation.Match(
            failures => failures,
            error => new[] { new ValidationFailure("", error.Message) }
        );
    }

    private static Eff<Option<T>> GetNonNullValueOrError<T>(T value, Error errorIfNull) =>
        Eff(() => Optional(value)).MapFail(_ => errorIfNull);

    private Eff<IValidator> GetValidatorFor(Type type) =>
        Eff(() => _validatorFactory.GetValidatorFor(type));

    private Aff<ValidationResult> GetValidationResultFor(IValidator validator, object value) =>
        AffMaybe<ValidationResult>(
            async () => await validator.ValidateAsync(new ValidationContext<object>(value))
        );

    private List<ValidationFailure> MapToValidationFailures(Error error) =>
        error.Code switch
        {
            404 => new List<ValidationFailure>(),
            _ => new List<ValidationFailure>(new[] { new ValidationFailure("", error.Message) })
        };

    private Aff<IEnumerable<ValidationFailure>> GetValidationResult(object value) =>
        AffMaybe<IEnumerable<ValidationFailure>>(async () =>
        {
            return (
                await (
                    from validator in GetValidatorFor(value.GetType())
                        .MapFail(_ => Error.New(404, "no validator"))
                    from validationResult in GetValidationResultFor(validator, value)
                        .MapFail(error => Error.New(500, error.Message, error.ToException()))
                    select validationResult
                ).Run()
            ).Match(result => result.Errors, MapToValidationFailures);
        });
}
