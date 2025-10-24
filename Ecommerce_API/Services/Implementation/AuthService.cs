using Ecommerce_API.Data;
using Ecommerce_API.DTOs.AuthDTO;
using Ecommerce_API.Models;
using Ecommerce_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return new AuthResponseDto(400, "Email already registered");
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create user entity
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = Roles.admin,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto(201, "User registered successfully");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return new AuthResponseDto(401, "Invalid credentials");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!verified)
            {
                return new AuthResponseDto(401, "Invalid credentials");
            }

            // Generate JWT tokens
            var accessToken = GenerateJwtToken(user);
            // For simplicity, refreshToken omitted but can be added similarly

            return new AuthResponseDto(200, "Login successful", accessToken, null);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}







