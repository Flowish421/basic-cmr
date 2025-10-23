using BasicCMR.Application.DTOs.Auth;

namespace BasicCMR.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginUserDto dto);
    }
}
