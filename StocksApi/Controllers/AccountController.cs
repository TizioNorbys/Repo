using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApi.Abstractions;
using StocksApi.IO.Requests;
using StocksApi.Persistence.Entities;

namespace Chat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IJwtProvider _jwtProvider;

    public AccountController(
		UserManager<AppUser> userManager,
		IJwtProvider jwtProvider)
	{
		_userManager = userManager;
		_jwtProvider = jwtProvider;
    }

    [HttpPost("[action]")]
	public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
	{
		var user = new AppUser
		{
			Id = Guid.NewGuid(),
			Email = request.Email,
			UserName = request.Email,
			Name = request.Name ?? string.Empty,
			LastName = request.LastName ?? string.Empty
		};

		await _userManager.CreateAsync(user, request.Password);
		return NoContent();
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
        var user = await _userManager.FindByEmailAsync(request.Email);
		if (user is null)
			return BadRequest(new {Message = "Invalid credentials"});

		if (!await _userManager.CheckPasswordAsync(user, request.Password))
			return BadRequest(new { Message = "Invalid credentials" });

        string token = _jwtProvider.Generate(user);
		return Ok(token);
    }
}