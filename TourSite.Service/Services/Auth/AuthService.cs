using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.User;
using TourSite.Core.DTOs.UserAuth;
using TourSite.Core.Entities;
using TourSite.Core.Servicies.Contract;

namespace TourSite.Service.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenService tokenService;
        public AuthService(UserManager<User> _userManager
            , SignInManager<User> _signInManager
            , ITokenService _tokenService
            )
        {
            userManager = _userManager;
            TokenService = _tokenService;
            signInManager = _signInManager;
        }

        public ITokenService TokenService { get; }

        private const string AdminEmail = "admin@gmail.com";



        public async Task<UserAuthDto> LoginAsync(LoginAuthDto loginAuthDto)
        {
            var user = await userManager.FindByEmailAsync(loginAuthDto.Email);

            if (user == null) return null;

            var result = await signInManager.CheckPasswordSignInAsync(user, loginAuthDto.Password, false);

            if (!result.Succeeded) return null;

            // =========================
            // AUTO ASSIGN ADMIN ROLE
            // =========================
            if (user.Email == AdminEmail)
            {
                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            var token = await TokenService.CreateTokenAsync(user, userManager);

            return new UserAuthDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = token
            };
        }
        public async Task<UserAuthDto> RegisterAsync(RegisterAuthDto registerAuthDto)
        {
            if (await CheckEmail(registerAuthDto.Email))
                return null;

            var user = new User
            {
                Email = registerAuthDto.Email,
                UserName = registerAuthDto.Email,
                FullName = registerAuthDto.FullName
            };

            var result = await userManager.CreateAsync(user, registerAuthDto.Password);

            if (!result.Succeeded)
                return null;

            // =========================
            // ASSIGN DEFAULT ROLE
            // =========================
            await userManager.AddToRoleAsync(user, "Customer");

            var token = await TokenService.CreateTokenAsync(user, userManager);

            return new UserAuthDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = token
            };
        }
        public async Task<bool> CheckEmail(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }
    }
}
