using System.Security.Claims;
using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Application.Services;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;
    
    public AuthController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request)
    {
        var registrationDto = _mapper.Map<UserRegistrationDto>(request);
        var userDto = await _userService.RegisterUserAsync(registrationDto);
        return Ok(_mapper.Map<UserResponse>(userDto));
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserRequest request)
    {
        var (accessToken, refreshToken) = await _userService.AuthenticateAsync(request.Email, request.Password);
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.Strict
        });
        return Ok(new { AccessToken = accessToken });
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var (newAccessToken, newRefreshToken) = await _userService.RefreshTokenAsync(refreshToken);
        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.Strict
        });
        return Ok(new { AccessToken = newAccessToken });
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    
        await _userService.RevokeRefreshTokenAsync(userId);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    
        return NoContent();
    }
    
    
    
    
}