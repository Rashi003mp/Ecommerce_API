using Ecommerce_API.Data;
using Ecommerce_API.DTOs.AuthDTO;
using Ecommerce_API.Models;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IConfiguration _configuration;

        public AuthService(IGenericRepository<User> userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                    throw new ArgumentNullException(nameof(registerDto), "Register request cannot be null");

                // Normalize input
                registerDto.Email = registerDto.Email.Trim().ToLower();
                registerDto.Name = registerDto.Name.Trim();
                registerDto.Password = registerDto.Password.Trim();

                // Check if user already exists
                var existingUser = (await _userRepo.GetAllAsync())
                    .FirstOrDefault(u => u.Email == registerDto.Email);

                if (existingUser != null)
                    return new AuthResponseDto(409, "Email already registered");

                // Hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Create user entity
                var newUser = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    Role = Roles.admin,     
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                // Save user via repository
                await _userRepo.AddAsync(newUser);

                return new AuthResponseDto(201, "User registered successfully");
            }
            catch (Exception ex)
            {
                return new AuthResponseDto(500, $"Error registering user: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                if (loginDto == null)
                    throw new ArgumentNullException(nameof(loginDto), "Login request cannot be null");

                var user = (await _userRepo.GetAllAsync())
                    .FirstOrDefault(u => u.Email == loginDto.Email.Trim().ToLower());

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password.Trim(), user.PasswordHash))
                    return new AuthResponseDto(401, "Invalid credentials");

                // Generate JWT token
                var token = GenerateJwtToken(user);

                return new AuthResponseDto(200, "Login successful", token, null);
            }
            catch (Exception ex)
            {
                return new AuthResponseDto(500, $"Error during login: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role.ToString())
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
