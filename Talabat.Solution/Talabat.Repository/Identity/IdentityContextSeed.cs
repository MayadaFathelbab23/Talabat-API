using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.Identity
{
    public static class IdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Mayada Mohamed",
                    Email = "mayadamohamedfathy206@gmail.com",
                    UserName = "mayadamohamedfathy206",
                    PhoneNumber = "01157353186",
                };
                await userManager.CreateAsync(user, "Pa$$w0rd"); 
            }
        }
    }
}
