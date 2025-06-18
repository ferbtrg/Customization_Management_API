using Customization_Management_API.Application.DTOs;

namespace Customization_Management_API.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginDto loginDto);
    }
}
