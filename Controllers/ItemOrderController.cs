using System;
using System.ComponentModel.DataAnnotations;
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
    public class ItemOrderController : ControllerBase
    {
        private readonly IItemOrderService _itemOrderService;

        public ItemOrderController(IItemOrderService itemOrderService)
        {
            _itemOrderService = itemOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _itemOrderService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _itemOrderService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ItemOrderCreateDto itemOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemOrder = new ItemOrder
            {
                FkProductId = itemOrderDto.FkProductId,
                FkOrderId = itemOrderDto.FkOrderId,
                Quantity = itemOrderDto.Quantity,
                ItemValue = itemOrderDto.ItemValue
            };

            var result = await _itemOrderService.AddAsync(itemOrder);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = itemOrder.Id }, itemOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ItemOrderUpdateDto itemOrderDto)
        {
            if (id != itemOrderDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _itemOrderService.UpdateAsync(itemOrderDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _itemOrderService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}


public class ItemOrderCreateDto
{
    [Required]
    public Guid FkProductId { get; set; }

    [Required]
    public Guid FkOrderId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal ItemValue { get; set; }
}

public class ItemOrderUpdateDto : ItemOrderCreateDto
{
    [Required]
    public Guid Id { get; set; }
}

