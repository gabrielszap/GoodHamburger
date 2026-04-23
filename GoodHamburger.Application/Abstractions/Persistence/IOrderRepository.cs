using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<ListOrderResult> GetAllAsync(CancellationToken cancellationToken);
    Task<OrderResult> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<OrderResult> AddAsync(Order order, CancellationToken cancellationToken);
    Task UpdateAsync(Order order, CancellationToken cancellationToken);
    Task DeleteAsync(Guid orderId, CancellationToken cancellationToken);
}
