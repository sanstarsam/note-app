using Microsoft.IdentityModel.Tokens;
using NoteApp.Models;
using NoteApp.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteApp.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string username, string password);
        Task<string?> LoginAsync(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<User?> RegisterAsync(string username, string password)
        {
            var existing = await _userRepository.GetByUsernameAsync(username);
            if (existing != null) return null;

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { user_name = username, password_hash = hash };
            user.id = await _userRepository.CreateAsync(user);
            return user;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.password_hash))
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.user_name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
