using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.ValidateUserAsync(loginDto.Username, loginDto.Password);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var token = await _tokenService.GenerateTokenAsync(user);
        return Ok(new { token });
    }
}

public class LoginDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
