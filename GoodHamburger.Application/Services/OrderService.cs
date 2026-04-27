using FluentValidation;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Mappers;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

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

        public async Task<IReadOnlyCollection<OrderResult>> GetAllAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(cancellationToken);
            return OrderMapper.ToResultList(orders).ToList();
        }
        public async Task<OrderResult> CreateAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(orderRequest, cancellationToken);

            var products = await _productRepository.GetByIdsAsync(
                orderRequest.ProductIds,
                cancellationToken);

            if (products.Count != orderRequest.ProductIds.Count)
                throw new NotFoundException("Um ou mais produtos informados não existem.");

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

            if (products.Count != orderRequest.ProductIds.Count)
                throw new NotFoundException("Um ou mais produtos informados não existem.");

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
