using FluentValidation;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.Validators.Auth;

public sealed class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("Data do pedido é obrigatória.")
            .Must(date => date != default).WithMessage("Data do pedido deve ser uma data válida.");

        RuleFor(x => x.Products)
            .Must(products =>
            {
                if (products == null) return true;

                return products
                    .GroupBy(p => p.Type)
                    .All(g => g.Count() <= 1);
            })
            .WithMessage("O pedido pode conter no máximo um item por categoria (sanduíche, acompanhamento e/ou bebida).");
    }
}
