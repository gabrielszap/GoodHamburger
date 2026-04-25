using GoodHamburger.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Contracts
{
    public interface IProductService
    {
        Task<ListProductResult> GetAllAsync(CancellationToken cancellationToken);
        Task<ProductResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ProductResult> CreateAsync(ProductRequest productRequest, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, ProductRequest productRequest, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
