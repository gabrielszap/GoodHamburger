using FluentValidation;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Mappers;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Application.DTOs.Common;

namespace GoodHamburger.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IValidator<OrderRequest> _validator;
        private readonly IApplicationUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IValidator<OrderRequest> validator, IApplicationUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
            return OrderMapper.ToResult(order);
        }

        public async Task<PagedResult<OrderResult>> GetAllAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(pagination.Take, pagination.Skip, cancellationToken);

            var totalItems = await _orderRepository.CountAsync(cancellationToken);

            var result = new PagedResult<OrderResult>
            {
                Items = orders.Select(OrderMapper.ToResult).ToList(),
                Page = pagination.Page,
                PageSize = pagination.Take,
                TotalItems = totalItems
            };

            return result;
        }
        public async Task<OrderResult> CreateAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(orderRequest, cancellationToken);

            var products = await _productRepository.GetByIdsAsync(
                orderRequest.ProductIds,
                cancellationToken);

            // ids retornados do banco
            var foundIds = products.Select(p => p.Id).ToHashSet();

            // ids que vieram na request mas não existem no banco
            var missingIds = orderRequest.ProductIds
                .Where(id => !foundIds.Contains(id))
                .ToList();

            if (missingIds.Any())
            {
                var message = $"Os seguintes produtos não foram encontrados: {string.Join(", ", missingIds)}";
                throw new NotFoundException(message);
            }

            var order = Order.Create(products);

            var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OrderMapper.ToResult(createdOrder);
        }

        public async Task UpdateAsync(Guid orderId, OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(orderRequest, cancellationToken);

            var existingOrder = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (existingOrder is null)
                throw new NotFoundException($"Pedido {orderId} não encontrado.");

            var products = await _productRepository.GetByIdsAsync(
                orderRequest.ProductIds,
                cancellationToken);

            // ids retornados do banco
            var foundIds = products.Select(p => p.Id).ToHashSet();

            // ids que vieram na request mas não existem no banco
            var missingIds = orderRequest.ProductIds
                .Where(id => !foundIds.Contains(id))
                .ToList();

            if (missingIds.Any())
            {
                var message = $"Os seguintes produtos não foram encontrados: {string.Join(", ", missingIds)}";
                throw new NotFoundException(message);
            }

            var order = Order.Create(products);

            await _orderRepository.UpdateAsync(orderId, order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var orderResult = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (orderResult is null)
                throw new NotFoundException($"Pedido {orderId} não encontrado.");

            await _orderRepository.DeleteAsync(orderId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
