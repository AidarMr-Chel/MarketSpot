using Microsoft.AspNetCore.Identity;
using MarketSpot.Core.Entities;

public static class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = new[] { "Admin", "Author", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
    {
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin123!";
        string adminRole = "Admin";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to create admin user");
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}
