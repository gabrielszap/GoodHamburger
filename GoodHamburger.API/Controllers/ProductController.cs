using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            var result = await _productService.CreateAsync(productRequest, cancellationToken);
            return Created(new Uri(""), result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            await _productService.UpdateAsync(productRequest, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
