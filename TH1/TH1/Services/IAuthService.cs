using TH1.DTOs;
using TH1.Models;

namespace TH1.Services
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}
