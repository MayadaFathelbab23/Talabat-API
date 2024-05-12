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
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager)
        {
            // Payload  - Private
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var Roles = await userManager.GetRolesAsync(user);

            foreach (var role in Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Key
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            // create jwt object
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssure"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256)
                ); 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
