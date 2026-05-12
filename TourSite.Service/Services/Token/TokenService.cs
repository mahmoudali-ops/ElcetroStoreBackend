using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Servicies.Contract;

namespace TourSite.Service.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration _configuration )
        {
            configuration = _configuration;
        }
        public async Task<string> CreateTokenAsync(User user, UserManager<User> userManager)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])
            );

            var signinCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            // =========================
            // ROLES
            // =========================
            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddDays(1),
                claims: claims,
                signingCredentials: signinCred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
