using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders.Data); // Ensure orders is an array or list
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderCreateDto orderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                Description = orderDto.Description,
                TotalValue = orderDto.TotalValue,
                ShippingDate = orderDto.ShippingDate,
                CreationDate = DateTime.UtcNow,
                ExpectedDeliveryDate = orderDto.ExpectedDeliveryDate,
                State = orderDto.State,
                NInstallments = orderDto.NInstallments,
                FkUserId = Guid.Parse(userId),  
                FkClientId = orderDto.FkClientId
            };

            var result = await _orderService.AddAsync(order);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] OrderUpdateDto orderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            if (id != orderDto.Id || !ModelState.IsValid)
                return BadRequest();

            orderDto.FkUserId = Guid.Parse(userId);

            var result = await _orderService.UpdateAsync(orderDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }

        [HttpGet("report/orders-by-period")]
        public async Task<IActionResult> GetOrderReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string status)
        {
            var report = await _orderService.GetOrderReportAsync(startDate, endDate, status);
            return Ok(report);
        }
    }
}


// OrderCreateDto
public class OrderCreateDto
{
    [StringLength(512)]
    public string Description { get; set; }

    [Required]
    public decimal TotalValue { get; set; }

    public DateTime? ShippingDate { get; set; }

    [Required]
    public DateTime ExpectedDeliveryDate { get; set; }

    [Required, StringLength(50)]
    public string State { get; set; }

    [Required]
    public int NInstallments { get; set; }

    [Required]
    public Guid FkClientId { get; set; }
}

public class OrderUpdateDto : OrderCreateDto
{
    [Required]
    public Guid Id { get; set; }

    public DateTime? DeliveryDate { get; set; }
    [Required]
    public Guid FkUserId { get; set; }
}

public class OrderReportDto
{
    public Guid OrderId { get; set; }
    public string ClientName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalValue { get; set; }
    public string Status { get; set; }
    public List<string> Products { get; set; }
}
