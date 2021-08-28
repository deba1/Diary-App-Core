using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public interface IAuthenticationManager
    {
        Task<UserViewModel> ValidateCredential(AuthCredential credential);
        Task<string> CreateToken();
        Task<IdentityResult> RegisterUser(RegisterViewModel user);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private IdentityUser _user;

        public AuthenticationManager(UserManager<IdentityUser> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }

        public async Task<string> CreateToken()
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("secret").Value));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("issuer").Value,
                audience: jwtSettings.GetSection("audience").Value,
                claims: await GetClaims(),
                expires: DateTime.Now.AddHours(3),
                signingCredentials: sign
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.Id)
            };
            var roles = await userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<UserViewModel> ValidateCredential(AuthCredential credential)
        {
            _user = await userManager.FindByNameAsync(credential.Username);
            var passwordMatched = await userManager.CheckPasswordAsync(_user, credential.Password);
            return _user != null && passwordMatched ? new UserViewModel
            {
                Id = _user.Id,
                Email = _user.Email,
                Roles = await userManager.GetRolesAsync(_user),
                Username = _user.UserName
            } : null;
        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel user)
        {
            var ph = new PasswordHasher<IdentityUser>();
            var newUser = new IdentityUser
            {
                Email = user.Email,
                UserName = user.Username
            };

            newUser.PasswordHash = ph.HashPassword(newUser, user.Password);

            var createdUser = await userManager.CreateAsync(newUser);

            await userManager.AddToRoleAsync(newUser, "Regular");
            return createdUser;
        }
    }
}
