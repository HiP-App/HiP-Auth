using Auth.Models;
using Auth.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Data
{
    public static class DbTask
    {
        public static void RunMigrationsAndSeedDb(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            var appConfig = app.ApplicationServices.GetRequiredService<AppConfig>();

            // Run Migrations to apply any pending migrations. (if any)
            dbContext.Database.Migrate();

            // Seed Db with Admin Account if its not already created.
            var admin = userManager.FindByEmailAsync(appConfig.AdminCredentials.Email).Result;

            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = appConfig.AdminCredentials.Email,
                    Email = appConfig.AdminCredentials.Email
                };

                userManager.CreateAsync(admin, appConfig.AdminCredentials.Password);
            }
        }
    }
}