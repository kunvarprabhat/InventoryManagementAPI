using Microsoft.AspNetCore.Identity;

namespace InventoryManagementAPI.Data
{

    public static class DbInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.CreateAsync(adminRole).Wait();
            }

            if (!roleManager.RoleExistsAsync("Supervisor").Result)
            {
                var supervisorRole = new IdentityRole("Supervisor");
                roleManager.CreateAsync(supervisorRole).Wait();
            }

            if (userManager.FindByEmailAsync("admin@inventory.com").Result == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@inventory.com",
                    Email = "admin@inventory.com"
                };
                userManager.CreateAsync(adminUser, "Admin@123").Wait();
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }
        }
    }
}
