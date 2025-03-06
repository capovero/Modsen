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
        return Ok(new { 
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(
        [FromBody] RefreshTokenRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest("Invalid refresh token");
        }

        var (newAccessToken, newRefreshToken) = await _userService.RefreshTokenAsync(request.RefreshToken);
    
        return Ok(new { 
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken 
        });
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _userService.RevokeRefreshTokenAsync(userId);
        return NoContent();
    }
    
    
    
    
}