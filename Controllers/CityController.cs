using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _cityService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CityCreateDto cityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = new City
            {
                Name = cityDto.Name,
                StateUF = cityDto.StateUF
            };

            var result = await _cityService.AddAsync(city);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(Get), new { id = city.Id }, city);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CityUpdateDto cityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cityService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var existingCity = result.Data;
            existingCity.Name = cityDto.Name;
            existingCity.StateUF = cityDto.StateUF;

            var updateResult = await _cityService.UpdateAsync(existingCity);
            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _cityService.DeleteAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }
    }
}

public class CityCreateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(2)]
    public string StateUF { get; set; }
}

public class CityUpdateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(2)]
    public string StateUF { get; set; }
}
