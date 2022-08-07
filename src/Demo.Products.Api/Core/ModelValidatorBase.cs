using FluentValidation;
using FluentValidation.Results;

namespace Demo.Products.Api.Core;

public abstract class ModelValidatorBase<T> : AbstractValidator<T>
{
    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        if (context.InstanceToValidate == null)
        {
            result.Errors.Add(new ValidationFailure("","null instance"));
            return false;
        }
        
        return base.PreValidate(context, result);
    }
}