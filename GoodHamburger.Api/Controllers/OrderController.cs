using Microsoft.AspNetCore.Mvc;
using Good_Hamburger.Models;
using Good_Hamburger.Services;

namespace Good_Hamburger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound(new { message = "Pedido não encontrado." });
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequestDto request)
        {
            try
            {
                if (request.FoodIds == null || !request.FoodIds.Any())
                {
                    return BadRequest(new { message = "O pedido deve conter ao menos um item." });
                }

                var order = await _orderService.CreateOrderAsync(request.FoodIds);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
