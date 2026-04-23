using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<ListProductResult> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<ProductResult> AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
}
    