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

    public async Task<IReadOnlyCollection<Order>> GetAllAsync(
    int take,
    int skip,
    CancellationToken cancellationToken)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);

        var orderRecords = (await connection.QueryAsync<OrderRecord>(
            new CommandDefinition(
                """
            select
                id as "Id",
                is_active as "IsActive",
                created_at as "CreatedAt"
            from "order"
            where is_active = true
            order by created_at desc
            limit @Take offset @Skip;
            """,
                new
                {
                    Take = take,
                    Skip = skip
                },
                cancellationToken: cancellationToken)))
            .ToList();

        if (!orderRecords.Any())
            return [];

        var rows = (await connection.QueryAsync<OrderProductRow>(
            new CommandDefinition(
                """
            select
                op.order_id as "OrderId",
                p.id as "ProductId",
                p.description as "Description",
                p.price as "Price",
                p.type as "Type",
                p.is_active as "ProductIsActive",
                p.created_at as "CreatedAt"
            from order_product op
            inner join "product" p
                on p.id = op.product_id
            where op.order_id = any(@OrderIds)
              and op.is_active = true;
            """,
                new
                {
                    OrderIds = orderRecords.Select(o => o.Id).ToArray()
                },
                cancellationToken: cancellationToken)))
            .ToList();

        var productsByOrderId = rows
            .GroupBy(x => x.OrderId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Where(x => x.ProductId.HasValue)
                    .Select(x => Product.Create(
                        x.ProductId!.Value,
                        x.Description!,
                        x.Price!.Value,
                        x.Type,
                        x.ProductIsActive!.Value,
                        x.CreatedAt))
                    .ToList());

        var orders = orderRecords
            .Select(orderRecord =>
            {
                productsByOrderId.TryGetValue(orderRecord.Id, out var products);

                return Order.Create(
                    orderRecord.Id,
                    products ?? [],
                    orderRecord.IsActive,
                    orderRecord.CreatedAt);
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
                p.is_active as "IsActive",
                P.created_at as "CreatedAt"
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

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                """
            select count(*)
            from "order";
            """,
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

    private sealed class ProductRecord
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
        public bool IsActive { get; set; }
    }


}
