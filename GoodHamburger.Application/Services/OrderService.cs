using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Order;

namespace GoodHamburger.Application.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderResult> CreateOrder(OrderRequest order)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> CreateOrder(OrderRequest order, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrder(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ListOrderResult> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<ListOrderResult> GetOrders(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrder(OrderRequest order)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrder(OrderRequest order, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
