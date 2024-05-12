using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models.Identity;

namespace Talabat.APIs.Extentions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> GetUserByEmailWithAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user =  await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == Email);
            return user;
        }
    }
}
