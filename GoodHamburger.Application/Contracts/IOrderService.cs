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
        Task<OrderResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ListOrderResult> GetAllAsync(CancellationToken cancellationToken);
        Task<OrderResult> CreateAsync(OrderRequest order, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, OrderRequest order, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
