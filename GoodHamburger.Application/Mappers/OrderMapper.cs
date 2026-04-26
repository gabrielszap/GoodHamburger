using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderResult ToResult(Order order)
        {
            return new OrderResult
            {
                Id = order.Id,
                IsActive = order.IsActive,
                CreatedAt = order.CreatedAt,

                Products = order.Products.Select(product => new ProductResult
                {
                    Id = product.Id,
                    Description = product.Description,
                    Price = product.Price,
                    Type = product.Type,
                    IsActive = product.IsActive
                }).ToList(),

                Subtotal = order.Subtotal,
                DiscountPercentage = order.DiscountPercentage,
                DiscountAmount = order.DiscountAmount,
                TotalPrice = order.Total
            };
        }

        public static IReadOnlyCollection<OrderResult> ToResultList(IEnumerable<Order> orders)
        {
            return orders.Select(ToResult).ToList();
        }
    }
}
