using GoodHamburger.Application.Contracts;
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

        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrders(cancellationToken);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrderById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            var result = await _orderService.CreateOrder(orderRequest, cancellationToken);
            return Created(new Uri(""), result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            await _orderService.UpdateOrder(orderRequest, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.DeleteOrder(id, cancellationToken);
            return NoContent();
        }
    }
}
