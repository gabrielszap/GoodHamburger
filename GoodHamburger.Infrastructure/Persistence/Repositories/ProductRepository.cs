using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Persistence.UnitOfWork;

namespace GoodHamburger.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : DapperRepositoryBase, IProductRepository
{

    public ProductRepository(DapperDbSession dbSession)
        : base(dbSession)
    {
    }

    public async Task<ListProductResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductResult> AddAsync(Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}
