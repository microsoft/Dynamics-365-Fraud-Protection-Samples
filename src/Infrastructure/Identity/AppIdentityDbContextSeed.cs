// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Contoso.FraudProtection.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var defaultUser = new ApplicationUser { UserName = "demoadminuser@microsoft.com", Email = "demoadminuser@microsoft.com" };
                var user = await userManager.CreateAsync(defaultUser, "Pass@word1");
                await userManager.AddToRoleAsync(userManager.Users.FirstOrDefault(), "Manager");

                defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
                await userManager.CreateAsync(defaultUser, "Pass@word1");
            }
        }

        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole { Id = "4B577B17-93A1-49DD-8E86-B94DD29F6908", Name = "Manager", NormalizedName = "Manager" };
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
