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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email
            };

            var result = await _userService.AddAsync(user);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserUpdateDto userDto)
        {
            if (id != userDto.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _userService.UpdateAsync(userDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}

public class UserCreateDto
{
    [Required, StringLength(50)]
    public string Username { get; set; }

    [Required, StringLength(200)]
    public string Password { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }
}

public class UserUpdateDto : UserCreateDto
{
    [Required]
    public Guid Id { get; set; }
}

