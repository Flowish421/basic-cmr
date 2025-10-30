using BasicCMR.Application.DTOs.Auth;
using BasicCMR.Domain.Entities;
using BasicCMR.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicCMR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 🔹 Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("E-postadressen används redan.");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Användare registrerad framgångsrikt ✅",
                user.Email
            });
        }

        // 🔹 Login and get JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Fel e-post eller lösenord ❌");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // 🔹 Generate JWT Token (null-säkert)
        private string GenerateJwtToken(User user)
        {
            // ✅ Null-checks med tydliga felmeddelanden
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("❌ JWT Key saknas i appsettings.json");

            var jwtIssuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("❌ JWT Issuer saknas i appsettings.json");

            var jwtAudience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("❌ JWT Audience saknas i appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
