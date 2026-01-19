using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TH1.DTOs;
using TH1.Models;
using TH1.Repositories;
using TH1.Patterns.AbstractFactory;
using TH1.Patterns.Singleton;
namespace TH1.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly INotificationFactory _notificationFactory;


        public AuthService(IUserRepository userRepository, IConfiguration configuration, INotificationFactory notificationFactory)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _notificationFactory = notificationFactory;
        }

        public async Task<User> Register(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "USER"
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            var notification = _notificationFactory.CreateNotification();
            notification.SendRegistrationSuccessNotification(user.Email);
            LoggerService.Instance.Log($"User {user.Username} registered successfully.");


            return user;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials");
            }
            LoggerService.Instance.Log($"User {user.Username} logged in successfully.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
