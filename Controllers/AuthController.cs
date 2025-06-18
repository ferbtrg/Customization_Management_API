using Customization_Management_API.Application.DTOs;
using Customization_Management_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customization_Management_API.Controllers;

/// <summary>
/// Controller responsible for handling authentication-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// 200 OK with JWT token if authentication is successful
    /// - 400 Bad Request if credentials are invalid
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login( LoginDto loginDto )
    {
        var response = await _authService.Login(loginDto);
        if( response is null )
            return BadRequest( "Invalid login." );

        return Ok( response );
    }
} 