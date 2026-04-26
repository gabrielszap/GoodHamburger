using Dapper;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Infrastructure.Persistence.UnitOfWork;

namespace GoodHamburger.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : DapperRepositoryBase, IOrderRepository
{

    public OrderRepository(DapperDbSession dbSession)
        : base(dbSession)
    {
    }

    public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<OrderProductRow>(
        new CommandDefinition(
            """
            select
                o.id as "OrderId",
                o.is_active as "OrderIsActive",
                o.created_at as "CreatedAt",

                p.id as "ProductId",
                p.description as "Description",
                p.price as "Price",
                p.type as "Type",
                p.is_active as "ProductIsActive"
            from "order" o
            left join order_product op
                on op.order_id = o.id
               and op.is_active = true
            left join "product" p
                on p.id = op.product_id
               and p.is_active = true
            where o.is_active = true
            order by o.created_at desc, p.type;
            """,
            cancellationToken: cancellationToken));

        var orders = rows
            .GroupBy(x => new
            {
                x.OrderId,
                x.OrderIsActive,
                x.CreatedAt
            })
            .Select(group =>
            {
                var products = group
                    .Where(x => x.ProductId.HasValue)
                    .Select(x => new Product
                    {
                        Id = x.ProductId!.Value,
                        Description = x.Description!,
                        Price = x.Price!.Value,
                        Type = x.Type,
                        IsActive = x.ProductIsActive!.Value
                    })
                    .ToList();

                return Order.Create(group.Key.OrderId, products.ToList(), group.Key.OrderIsActive, group.Key.CreatedAt);
            })
            .ToList();

        return orders;
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var orderRecord = await connection.QuerySingleOrDefaultAsync<OrderRecord>(
            new CommandDefinition(
                """
            select
                id as "Id",
                is_active as "IsActive",
                created_at as "CreatedAt"
            from "order"
            where id = @Id
              and is_active = true;
            """,
                new { Id = id },
                cancellationToken: cancellationToken));

        if (orderRecord is null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado.");

        var products = await connection.QueryAsync<Product>(
            new CommandDefinition(
                """
            select
                p.id as "Id",
                p.description as "Description",
                p.price as "Price",
                p.type as "Type",
                p.is_active as "IsActive"
            from order_product op
            inner join "product" p
                on p.id = op.product_id
            where op.order_id = @Id
              and op.is_active = true
              and p.is_active = true;
            """,
                new { Id = id },
                cancellationToken: cancellationToken));

        return Order.Create(orderRecord.Id, products.ToList(), orderRecord.IsActive, orderRecord.CreatedAt);
        
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var record = await connection.QuerySingleAsync<OrderRecord>(new CommandDefinition(
            """
                insert into "order" default values

                returning
                id AS "Id",
                created_at AS "CreatedAt",
                is_active AS "IsActive";
                """,
            Transaction,
            cancellationToken: cancellationToken));

        foreach (var product in order.Products)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    insert into order_product (order_id, product_id)
                    values (@OrderId, @ProductId);
                    """,
                    new
                    {
                        OrderId = record.Id,
                        ProductId = product.Id
                    },
                    Transaction,
                    cancellationToken: cancellationToken));
        }

        return await GetByIdAsync(record.Id, cancellationToken);
    }

    public async Task UpdateAsync(Guid orderId, Order order, CancellationToken cancellationToken)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update "order_product"
                set
                    is_active = false
                where order_id = @Id;
                """,
                new
                {
                    Id = orderId,
                },
                cancellationToken: cancellationToken));

        foreach (var product in order.Products)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    insert into order_product (order_id, product_id)
                    values (@OrderId, @ProductId);
                    """,
                    new
                    {
                        OrderId = orderId,
                        ProductId = product.Id
                    },
                    Transaction,
                    cancellationToken: cancellationToken));
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await EnsureTransactionAsync(cancellationToken);
        var connection = await GetOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update "order"
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

    private sealed class OrderRecord
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private sealed class OrderProductRow
    {
        public Guid OrderId { get; set; }
        public bool OrderIsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? ProductId { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public ProductType Type { get; set; }
        public bool? ProductIsActive { get; set; }
    }


}
