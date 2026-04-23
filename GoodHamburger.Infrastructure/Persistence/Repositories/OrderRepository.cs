using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Persistence.UnitOfWork;

namespace GoodHamburger.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : DapperRepositoryBase, IOrderRepository
{

    public OrderRepository(DapperDbSession dbSession)
        : base(dbSession)
    {
    }

    public Task<OrderResult> AddAsync(Order order, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ListOrderResult> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<OrderResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
