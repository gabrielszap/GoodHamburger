using Dapper;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
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
        var connection = await GetOpenConnectionAsync(cancellationToken);
        var records = await connection.QueryAsync<Product>(
            new CommandDefinition(
                """
                select
                    id as Id,
                    description as Description,
                    price as Price,
                    type as Type,
                    is_active as IsActive
                from product
                """,
                cancellationToken: cancellationToken));

        var result = new ListProductResult
        {
            Products = records.Select(p => new ProductResult
            {
                Id = p.Id,
                Description = p.Description,
                Price = p.Price,
                Type = p.Type,
                IsActive = p.IsActive
            }).ToList()
        };

        return result;
    }

    public async Task<ProductResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);
        var record = await connection.QueryFirstAsync<Product>(
            new CommandDefinition(
                """
                select
                    id as Id,
                    description as Description,
                    price as Price,
                    type as Type,
                    is_active as IsActive
                from product
                where id = @Id
                """,
                new { Id = id },
                cancellationToken: cancellationToken));

        var result = new ProductResult()
        {
            Id = record.Id,
            Description = record.Description,
            Price = record.Price,
            Type = record.Type,
            IsActive = record.IsActive
        };

        return result;
    }

    public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(
    IReadOnlyCollection<Guid> ids,
    CancellationToken cancellationToken)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var products = await connection.QueryAsync<Product>(
            new CommandDefinition(
                """
            select
                id as "Id",
                description as "Description",
                price as "Price",
                type as "Type",
                is_active as "IsActive"
            from "product"
            where id = any(@Ids)
              and is_active = true;
            """,
                new { Ids = ids.ToArray() },
                cancellationToken: cancellationToken));

        return products.ToList();
    }

    public async Task<ProductResult> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var record = await connection.QuerySingleAsync<Product>(new CommandDefinition(
            """
                insert into product (
                    description,
                    price,
                    type
                )
                values (
                    @Description,
                    @Price,
                    @Type
                )
                returning
                id AS "Id",
                description AS "Description",
                price AS "Price",
                type AS "Type";
                """,
            new
            {
                product.Description,
                product.Price,
                Type = product.Type.ToString()
            },
            Transaction,
            cancellationToken: cancellationToken));

        var result = new ProductResult()
        {
            Id = record.Id,
            Description = record.Description,
            Price = record.Price,
            Type = record.Type,
            IsActive = record.IsActive
        };

        return result;

    }

    public async Task UpdateAsync(Guid id, Product product, CancellationToken cancellationToken = default)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update product
                set
                    description = @Description,
                    price = @Price,
                    type = @Type
                where id = @Id;
                """,
                new { 
                    Id = id,
                    product.Description,
                    product.Price,
                    Type = product.Type.ToString()
                },
                cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update product
                set
                    is_active = false
                where id = @Id;
                """,
                new
                {
                    Id = id
                },
                cancellationToken: cancellationToken));
    }

}
