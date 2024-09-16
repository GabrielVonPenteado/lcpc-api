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
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _stateService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{uf}")]
        public async Task<IActionResult> Get(string uf)
        {
            var result = await _stateService.GetByUFAsync(uf);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StateCreateDto stateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var state = new State
            {
                UF = stateDto.UF,
                Name = stateDto.Name
            };

            var result = await _stateService.AddAsync(state);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { uf = state.UF }, state);
        }

        [HttpPut("{uf}")]
        public async Task<IActionResult> Put(string uf, [FromBody] StateUpdateDto stateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _stateService.UpdateAsync(uf, stateDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{uf}")]
        public async Task<IActionResult> Delete(string uf)
        {
            var result = await _stateService.DeleteAsync(uf);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}


public class StateCreateDto
{
    [Required, StringLength(2)]
    public string UF { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }
}

public class StateUpdateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }
}
