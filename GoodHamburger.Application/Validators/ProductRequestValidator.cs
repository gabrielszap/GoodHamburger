using FluentValidation;
using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.Validators;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Type).NotEmpty();
    }
}
