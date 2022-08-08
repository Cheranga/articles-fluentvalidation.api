using Demo.Products.Api.Core;
using FluentValidation;

namespace Demo.Products.Api.Features.AddProduct;

public class AddProductRequestDtoValidator : ModelValidatorBase<AddProductRequestDto>
{
    public AddProductRequestDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id is required");
        RuleFor(x => x.Name).MustAsync(async (s, token) =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1), token);
            return !string.IsNullOrWhiteSpace(s);
        }).WithMessage("name is required");
    }
}