using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _clientService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientCreateDto clientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Name = clientDto.Name,
                Streetplace = clientDto.Streetplace,
                Neighborhood = clientDto.Neighborhood,
                Number = clientDto.Number,
                Complement = clientDto.Complement,
                Phone = clientDto.Phone,
                Email = clientDto.Email,
                CNPJ = clientDto.CNPJ,
                FkCityId = clientDto.FkCityId
            };

            var result = await _clientService.AddAsync(client);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ClientUpdateDto clientDto)
        {
            if (id != clientDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _clientService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            var existingClient = result.Data;
            existingClient.Name = clientDto.Name;
            existingClient.Streetplace = clientDto.Streetplace;
            existingClient.Neighborhood = clientDto.Neighborhood;
            existingClient.Number = clientDto.Number;
            existingClient.Complement = clientDto.Complement;
            existingClient.Phone = clientDto.Phone;
            existingClient.Email = clientDto.Email;
            existingClient.CNPJ = clientDto.CNPJ;
            existingClient.FkCityId = clientDto.FkCityId;

            var updateResult = await _clientService.UpdateAsync(existingClient);

            if (!updateResult.Success)
                return BadRequest(updateResult.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _clientService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }

    // DTO para criação de cliente
    public class ClientCreateDto
    {
        public string Name { get; set; }
        public string Streetplace { get; set; }
        public string Neighborhood { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CNPJ { get; set; }
        public Guid FkCityId { get; set; }
    }

    // DTO para atualização de cliente
    public class ClientUpdateDto : ClientCreateDto
    {
        public Guid Id { get; set; }
    }
}
