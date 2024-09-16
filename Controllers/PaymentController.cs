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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _paymentService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentCreateDto paymentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payment = new Payment
            {
                DataPayment = paymentDto.DataPayment ?? DateTime.UtcNow,
                Value = paymentDto.Value,
                PaymentType = paymentDto.PaymentType,
                ReceivementType = paymentDto.ReceivementType,
                FkInstallmentId = paymentDto.FkInstallmentId,
                FkUserId = Guid.Parse(userId)  
            };

            var result = await _paymentService.AddAsync(payment);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PaymentUpdateDto paymentDto)
        {
            if (id != paymentDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _paymentService.UpdateAsync(paymentDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _paymentService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}


public class PaymentCreateDto
{
    public DateTime? DataPayment { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public PaymentTypeEnum PaymentType { get; set; }

    [Required]
    public ReceivementTypeEnum ReceivementType { get; set; }

    [Required]
    public Guid FkInstallmentId { get; set; } 

    [Required]
    public Guid FkUserId { get; set; } 
}

public class PaymentUpdateDto : PaymentCreateDto
{
    [Required]
    public Guid Id { get; set; }
}
