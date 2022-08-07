using Demo.Products.Api.Core;
using FluentValidation;

namespace Demo.Products.Api.Features.AddProduct;

public class AddProductRequestDtoValidator : ModelValidatorBase<AddProductRequestDto>
{
    public AddProductRequestDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id is required");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required");
    }
}