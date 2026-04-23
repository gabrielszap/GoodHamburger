using GoodHamburger.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Contracts
{
    public interface IOrderService
    {
        Task<OrderResult> GetOrderById(Guid id, CancellationToken cancellationToken);
        Task<ListOrderResult> GetOrders(CancellationToken cancellationToken);
        Task<OrderResult> CreateOrder(OrderRequest order, CancellationToken cancellationToken);
        Task UpdateOrder(OrderRequest order, CancellationToken cancellationToken);
        Task DeleteOrder(Guid id, CancellationToken cancellationToken);
    }
}
