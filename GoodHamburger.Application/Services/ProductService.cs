using FluentValidation;
using GoodHamburger.Application.Abstractions.Persistence;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Product;
using System.Threading;

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

            var product = new Domain.Entities.Product
            {
                Description = productRequest.Description,
                Price = productRequest.Price,
                Type = productRequest.Type
            };

            var result = await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ListProductResult> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ProductRequest productRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
