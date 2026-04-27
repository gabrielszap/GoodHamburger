using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> GetAllAsync(int take, int skip, CancellationToken cancellationToken);
    Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken);
    Task UpdateAsync(Guid orderId, Order order, CancellationToken cancellationToken);
    Task DeleteAsync(Guid orderId, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
}
