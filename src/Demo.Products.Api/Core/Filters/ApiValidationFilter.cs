using Demo.Products.Api.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Products.Api.Core.Filters;

public class ApiValidationFilter : IAsyncActionFilter
{
    private readonly ICustomValidatorFactory _validatorFactory;

    public ApiValidationFilter(ICustomValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.Any())
        {
            await next();
            return;
        }

        var validationFailures = new List<ValidationFailure>();
        foreach (var actionArgument in context.ActionArguments)
        {
            if (actionArgument.Value == null)
            {
                continue;
            }

            var validationResult = await ValidateAsync(actionArgument.Value);
            if (validationResult.IsValid)
            {
                continue;
            }

            validationFailures.AddRange(validationResult.Errors);
        }

        if (!validationFailures.Any())
        {
            await next();
            return;
        }

        context.Result = new BadRequestObjectResult(validationFailures.ToProblemDetails());
    }

    private async Task<ValidationResult> ValidateAsync(object value)
    {
        if (value == null)
        {
            return new ValidationResult();
        }

        var validatorInstance = _validatorFactory.GetValidatorFor(value.GetType());
        if (validatorInstance == null)
        {
            return new ValidationResult();
        }

        return await validatorInstance.ValidateAsync(new ValidationContext<object>(value));
    }
}