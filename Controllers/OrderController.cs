using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Enums;
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
                Status = orderDto.Status,
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

        [HttpGet("report")]
        public async Task<IActionResult> GetOrdersReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] OrderStatus? status = null)
        {
            var result = await _orderService.GetOrdersReportAsync(startDate, endDate, status);
            
            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpGet("billing-report")]
        public async Task<IActionResult> GetBillingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] Guid? clientId)
        {
            if (startDate == default || endDate == default)
            {
                return BadRequest("Start date and end date must be provided.");
            }

            var result = await _orderService.GenerateOrderBillingReportAsync(startDate, endDate, clientId);
            
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("top-sold-products")]
        public async Task<IActionResult> GetTopSoldProductsReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] ProductTypeEnum? productType)
        {
            var result = await _orderService.GenerateTopSoldProductsReportAsync(startDate, endDate, productType);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
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

    [Required]
    public OrderStatus Status { get; set; }

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

// OrderBillingReportDto.cs
public class OrderBillingReportDto
{
    public string ClientName { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalOrderValue { get; set; }
    public decimal AverageOrderValue { get; set; }
}

public class ProductSalesReportDto
{
    public string ProductName { get; set; }
    public int QuantitySold { get; set; }
    public decimal TotalSalesValue { get; set; }
    public decimal AverageSalesValue { get; set; }
    public string ProductType { get; set; }  // Para exibir o tipo do produto
}


