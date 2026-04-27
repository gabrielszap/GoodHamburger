using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Common;
using GoodHamburger.Application.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Retorna todos os pedidos.
        /// </summary>
        /// <response code="200">Lista de pedidos</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetAllAsync(pagination, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Retorna um pedido pelo identificador.
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <response code="200">Pedido encontrado</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <remarks>
        /// Cria um pedido baseado nos produtos informados.
        /// O pedido pode conter:
        /// - 1 Sanduíche
        /// - 1 Acompanhamento
        /// - 1 Bebida
        ///
        /// Regras de desconto:
        /// - Sanduíche + Batata + Refrigerante → 20%
        /// - Sanduíche + Refrigerante → 15%
        /// - Sanduíche + Batata → 10%
        /// </remarks>
        /// <response code="201">Pedido criado com sucesso</response>
        /// <response code="400">Erro de validação ou regra de negócio</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            var result = await _orderService.CreateAsync(orderRequest, cancellationToken);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <remarks>
        /// Substitui os produtos do pedido.
        /// As mesmas regras de criação são aplicadas.
        /// </remarks>
        /// <response code="204">Pedido atualizado com sucesso</response>
        /// <response code="400">Erro de validação ou regra de negócio</response>
        /// <response code="404">Pedido ou produto não encontrado</response>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            await _orderService.UpdateAsync(id, orderRequest, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Remove (inativa) um pedido.
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <response code="204">Pedido removido com sucesso</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
