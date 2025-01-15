using ECommerce.Api.Data;
using Ecommerce.library.Models;
using Ecommerce.library.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbScope _db;
        private readonly IConfiguration _config; // We'll need this for JWT secret

        public AuthService(AppDbScope db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<ServiceResponse> RegisterAsync(string email, string password)
        {
            // 1. Check if user already exists
            if (await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            // 2. Create new user
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new ServiceResponse
            {
                Success = true,
                Message = "Registration successful."
            };
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null; // invalid password

            // Generate the JWT
            return CreateToken(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // =========================================
        // Helper Methods
        // =========================================
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private string CreateToken(User user)
        {
            // We retrieve the JWT secret from appsettings
            var secretKey = _config.GetValue<string>("JWTSettings:SecretKey");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("JWTSettings:Issuer"),
                audience: _config.GetValue<string>("JWTSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
