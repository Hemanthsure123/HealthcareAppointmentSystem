using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Seed Roles
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Doctor"));
            await roleManager.CreateAsync(new IdentityRole("Patient"));

            // Create Admin User
            var adminEmail = "hemanthadmin@gmail.com";
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsApproved = true
            };

            if (userManager.Users.All(u => u.Id != adminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(adminEmail);
                if (user == null)
                {
                    await userManager.CreateAsync(adminUser, "Hemanth0001@");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}