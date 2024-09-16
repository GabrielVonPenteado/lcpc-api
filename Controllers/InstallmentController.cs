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
    public class InstallmentController : ControllerBase
    {
        private readonly IInstallmentService _installmentService;

        public InstallmentController(IInstallmentService installmentService)
        {
            _installmentService = installmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _installmentService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _installmentService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InstallmentCreateDto installmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var installment = new Installment
            {
                ExpirationDate = installmentDto.ExpirationDate,
                Value = installmentDto.Value,
                Situation = installmentDto.Situation,
                FkOrderId = installmentDto.FkOrderId
            };

            var result = await _installmentService.AddAsync(installment);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = installment.Id }, installment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] InstallmentUpdateDto installmentDto)
        {
            if (id != installmentDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _installmentService.UpdateAsync(installmentDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _installmentService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }

    public class InstallmentCreateDto
    {
        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public bool Situation { get; set; }

        [Required]
        public Guid FkOrderId { get; set; } 
    }

    public class InstallmentUpdateDto : InstallmentCreateDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
