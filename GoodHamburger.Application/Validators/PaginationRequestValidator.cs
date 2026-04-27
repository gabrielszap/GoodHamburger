using FluentValidation;
using GoodHamburger.Application.DTOs.Common;

namespace GoodHamburger.Application.Validators;

public sealed class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page deve ser maior ou igual a 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize deve ser maior ou igual a 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize não pode ser maior que 100.");
    }
}