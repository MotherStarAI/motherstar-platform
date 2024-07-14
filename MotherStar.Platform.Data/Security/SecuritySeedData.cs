using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotherStar.Platform.Domain.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Data.Security
{
    public class SecuritySeedData
    {
        private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "admin@motherstar.io",
            NormalizedEmail = "TECH@MOTHERSTAR.IO",
            NormalizedUserName = "TECH@MOTHERSTAR.IO",
            RoleList = [ "Administrator", "Manager" ],
            UserName = "admin@motherstar.io"
        },
        new SeedUser()
        {
            Email = "user@motherstar.io",
            NormalizedEmail = "USER@MOTHERSTAR.IO",
            NormalizedUserName = "USER@MOTHERSTAR.IO",
            RoleList = [ "User" ],
            UserName = "user@motherstar.io"
        },
    ];

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new SecurityDbContext(serviceProvider.GetRequiredService<DbContextOptions<SecurityDbContext>>());

            if (context.Users.Any())
            {
                return;
            }

            var userStore = new UserStore<AppUser>(context);
            var password = new PasswordHasher<AppUser>();

            using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = ["Administrator", "Manager", "User"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            foreach (var user in seedUsers)
            {
                var hashed = password.HashPassword(user, "Passw0rd!");
                user.PasswordHash = hashed;
                await userStore.CreateAsync(user);

                if (user.Email is not null)
                {
                    var appUser = await userManager.FindByEmailAsync(user.Email);

                    if (appUser is not null && user.RoleList is not null)
                    {
                        await userManager.AddToRolesAsync(appUser, user.RoleList);
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private class SeedUser : AppUser
        {
            public string[]? RoleList { get; set; }
        }
    }

}
