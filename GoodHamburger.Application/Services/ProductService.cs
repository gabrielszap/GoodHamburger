using FluentValidation;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Mappers;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Application.DTOs.Common;

namespace GoodHamburger.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<ProductRequest> _validator;
        private readonly IApplicationUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IValidator<ProductRequest> validator, IApplicationUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResult> CreateAsync(ProductRequest productRequest, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(productRequest, cancellationToken);

            var product = Product.Create(productRequest.Description, productRequest.Price, (ProductType)Enum.Parse(typeof(ProductType), productRequest.Type));

            var result = await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductMapper.ToResult(result);
        }

        public async Task<ProductResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            return ProductMapper.ToResult(product);
        }

        public async Task<PagedResult<ProductResult>> GetAllAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(pagination.Take, pagination.Skip, cancellationToken);

            var totalItems = await _productRepository.CountAsync(cancellationToken);

            var result = new PagedResult<ProductResult>
            {
                Items = products.Select(ProductMapper.ToResult).ToList(),
                Page = pagination.Page,
                PageSize = pagination.Take,
                TotalItems = totalItems
            };

            return result;
        }

        public async Task UpdateAsync(Guid id, ProductRequest productRequest, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(productRequest, cancellationToken);

            var productResult = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (productResult is null)
                throw new NotFoundException($"Produto {id} não encontrado.");

            var product = Product.Create(productRequest.Description, productRequest.Price, (ProductType)Enum.Parse(typeof(ProductType), productRequest.Type));

            await _productRepository.UpdateAsync(id, product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }



        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var productResult = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (productResult is null)
                throw new NotFoundException($"Produto {id} não encontrado.");

            await _productRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
