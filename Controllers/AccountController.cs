using Baigiamasis.Data;
using Baigiamasis.Dto;
using Baigiamasis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Baigiamasis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest("Username already exists");

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(dto.Password, salt);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash, user.Salt))
                return Unauthorized();

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var saltedPassword = $"{salt}{password}";
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashedPasswordBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashedPasswordBytes);
        }

        private bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hashedInputPassword = HashPassword(password, salt);

            return hashedInputPassword == storedHash;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
