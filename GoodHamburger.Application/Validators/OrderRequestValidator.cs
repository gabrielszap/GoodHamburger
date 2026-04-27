using FluentValidation;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.Validators;

public sealed class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("O pedido deve conter ao menos um produto.");

        RuleForEach(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("ProductId inválido.");
    }
}
