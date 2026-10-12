using Microsoft.AspNetCore.Identity;
using JENSENonline.Api.Models;

namespace JENSENonline.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole("Customer"));

            var adminEmail = "admin@jensen.se";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var customerEmail = "kevin@kund.se";
            var customer = await userManager.FindByEmailAsync(customerEmail);

            if (customer == null)
            {
                customer = new ApplicationUser
                {
                    Email = customerEmail,
                    UserName = customerEmail
                };

                await userManager.CreateAsync(customer, "Kund123!");
                await userManager.AddToRoleAsync(customer, "Customer");
            }

            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Name = "HP Laptop ", Price = 4599, ImageUrl = "/images/HP-Laptop.png" },
                    new Product { Name = "Datorväska Elite", Price = 999, ImageUrl = "/images/Datorväska-Elite.png" },
                    new Product { Name = "Laddare-universal", Price = 349, ImageUrl = "/images/Universal-laddare.png" }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
