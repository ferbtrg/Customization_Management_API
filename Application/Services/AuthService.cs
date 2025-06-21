using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Customization_Management_API.Application.DTOs;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Customization_Management_API.Application.Services;
public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    UserDbContext                   _context;

    public AuthService( IConfiguration configuration, UserDbContext context )
    {
        _configuration  = configuration;
        _context        = context;
    }

    public async Task<LoginResponseDto> Login( LoginDto loginDto )
    {
        //Simulate authentication
        var role                = loginDto.Username.ToLower() == "admin" ? "Admin" : "Client";
        var userId              = Guid.NewGuid();
        var token               = GenerateJwtToken(userId, role);

        var response            = new LoginResponseDto();
        response.UserId         = userId;
        response.AccessToken    = token;
        response.Role           = role;

        return await CreateTokenResponse(userId, role);
    }

    private async Task<LoginResponseDto> CreateTokenResponse( Guid userId, string role )
    {
        var token           = GenerateJwtToken(userId, role);

        var response            = new LoginResponseDto();
        response.UserId         = userId;
        response.AccessToken    = token;
        response.Role           = role;
       // response.RefreshToken   = await GenerateAndSaveRefreshTokenAsync(userId);

        return response;
    }

    //TODO: Deal with RefreshToken later
    /*
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(Guid user)
    {
        var refreshToken            = GenerateRefreshToken();
        user.RefreshToken           = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
    */
    private string GenerateJwtToken( Guid userId, string role )
    {
        var claims = new List<Claim>
        {
            new Claim( ClaimTypes.NameIdentifier, userId.ToString() ),
            new Claim( ClaimTypes.Role, role )
        };

        var key                 = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( _configuration.GetValue<string>("Jwt:Key")!) );

        var credentials         = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor     = new JwtSecurityToken(
            issuer:                 _configuration.GetValue<string>("Jwt:Issuer"),
            audience:               _configuration.GetValue<string>("Jwt:Audience"),
            claims:                 claims,
            expires:                DateTime.UtcNow.AddDays(1),
            signingCredentials:     credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
} 