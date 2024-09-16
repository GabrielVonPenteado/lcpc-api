using System;
using System.ComponentModel.DataAnnotations;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = productDto.Name,
                ProductType = productDto.ProductType,
                Description = productDto.Description,
                Value = productDto.Value,
                Thickness = productDto.Thickness,
                Width = productDto.Width,
                Length = productDto.Length
            };

            var result = await _productService.AddAsync(product);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ProductUpdateDto productDto)
        {
            if (id != productDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _productService.UpdateAsync(productDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}


public class ProductCreateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required]
    public ProductTypeEnum ProductType { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public decimal Thickness { get; set; }

    [Required]
    public decimal Width { get; set; }

    [Required]
    public decimal Length { get; set; }
}

public class ProductUpdateDto : ProductCreateDto 
{
    [Required]
    public Guid Id { get; set; }
}
