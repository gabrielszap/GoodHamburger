using GoodHamburger.API.Requests;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Common;
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

        /// <summary>
        /// Retorna o cardápio da lanchonete (menu) de forma paginada.
        /// </summary>
        /// <remarks>
        /// Este endpoint representa o menu disponível para pedidos, contendo:
        /// - Sanduíches
        /// - Acompanhamentos
        /// - Bebidas
        ///
        /// A paginação é controlada pelos parâmetros:
        /// - page: número da página (mínimo 1)
        /// - pageSize: quantidade de itens por página (máximo 100)
        /// </remarks>
        /// <param name="query">Parâmetros de paginação (page, pageSize)</param>
        /// <response code="200">Lista paginada de produtos do menu</response>
        /// <response code="400">Erro de validação nos parâmetros de paginação</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
        {
            var pagination = PaginationRequest.Create(query.Page, query.PageSize);

            var result = await _productService.GetAllAsync(pagination, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Retorna um produto pelo identificador.
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <response code="200">Produto encontrado</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <remarks>
        /// Utilizado para cadastrar itens no cardápio.
        /// </remarks>
        /// <response code="201">Produto criado com sucesso</response>
        /// <response code="400">Erro de validação</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            var result = await _productService.CreateAsync(productRequest, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <response code="204">Produto atualizado com sucesso</response>
        /// <response code="400">Erro de validação</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            await _productService.UpdateAsync(id, productRequest, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Remove (inativa) um produto.
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <response code="204">Produto removido com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
