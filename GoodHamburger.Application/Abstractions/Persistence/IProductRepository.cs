using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<ListProductResult> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Product>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<ProductResult> AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
}
    