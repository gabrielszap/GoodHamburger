using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(int take, int skip, CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<Product> GetByDescriptionAsync(string description, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Product>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
}
    