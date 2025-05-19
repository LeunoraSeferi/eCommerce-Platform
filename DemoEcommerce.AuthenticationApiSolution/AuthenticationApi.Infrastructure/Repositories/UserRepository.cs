using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class UserRepository(AuthenticationDbContext context, IConfiguration config, UserManager<AppUser> userManager) : IUser
    {
        private readonly AuthenticationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        public IConfiguration config { get; } = config;

        private async Task<AppUser?> GetUserByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<GetUserDTO> GetUser(int userid)
        {
            var user = await context.Users.FindAsync(userid);
            return user != null ? new GetUserDTO(
                user.Id,
                user.Name!,
                user.TelephoneNumber!,
                user.Adress!,
                user.Email!,
                user.Role!) : null!;
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(appUserDTO.Email);
            if (existingUser != null)
                return new Response(false, "You cannot use this email for registration");

            var newUser = new AppUser
            {
                UserName = appUserDTO.Email, // ose përdor appUserDTO.Name nëse do
                Email = appUserDTO.Email,
                Name = appUserDTO.Name,
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Adress = appUserDTO.Address,
                Role = appUserDTO.Role
            };

            var createResult = await _userManager.CreateAsync(newUser, appUserDTO.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(" | ", createResult.Errors.Select(e => e.Description));
                return new Response(false, $"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(newUser, appUserDTO.Role);
            return new Response(true, "User registered successfully");
        }
        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return new Response(false, "Invalid credentials");

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!passwordValid)
                return new Response(false, "Invalid credentials");

            var token = GenerateToken(user, config);
            return new Response(true, token);
        }
        private static string GenerateToken(AppUser user, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["Authentication:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name!),
        new Claim(ClaimTypes.Email, user.Email!)
    };

            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


