namespace Customization_Management_API.Application.DTOs;
public class LoginDto
{
    public required string Username{ get; set; }
    public required string Password{ get; set; }
}

public class LoginResponseDto
{
    public Guid UserId{ get; set; }
    public string AccessToken{ get; set; } = string.Empty;
    public string Role{ get; set; } = string.Empty;
} 