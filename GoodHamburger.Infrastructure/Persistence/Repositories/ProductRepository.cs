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

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);
        var products = await connection.QueryAsync<Product>(
            new CommandDefinition(
                """
                select
                    id as Id,
                    description as Description,
                    price as Price,
                    type as Type,
                    is_active as IsActive,
                    created_at as CreatedAt
                from product
                """,
                cancellationToken: cancellationToken));        

        return products.ToList() ?? new List<Product>();
    }

    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);
        var product = await connection.QueryFirstAsync<Product>(
            new CommandDefinition(
                """
                select
                    id as Id,
                    description as Description,
                    price as Price,
                    type as Type,
                    is_active as IsActive,
                    created_at as CreatedAt
                from product
                where id = @Id
                """,
                new { Id = id },
                cancellationToken: cancellationToken));

        return product;
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

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var result = await connection.QuerySingleAsync<Product>(new CommandDefinition(
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
                type AS "Type",
                is_active AS "IsActive",
                created_at AS "CreatedAt";
                """,
            new
            {
                product.Description,
                product.Price,
                Type = product.Type.ToString()
            },
            Transaction,
            cancellationToken: cancellationToken));
        
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
                    type = @Type,
                    is_active = @IsActive
                where id = @Id;
                """,
                new { 
                    Id = id,
                    product.Description,
                    product.Price,
                    Type = product.Type.ToString(),
                    IsActive = product.IsActive
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
