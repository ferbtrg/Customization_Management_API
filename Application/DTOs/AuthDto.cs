
namespace Customization_Management_API.Application.DTOs;
public class LoginDto
{
    public required string Username{ get; set; }

    public required string Password{ get; set; }
}

public class LoginResponseDto
{
    public string AccessToken{ get; set; }
    public string RefreshToken { get; set; }
    public string Role{ get; set; }
    public Guid UserId{ get; set; }
} 