using Auth.Models;
using Auth.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Data
{
    public static class DbSeed
    {
        // TODO: Move this code when seed data is implemented in EF 7

        public static void SeedDbWithAdministrator(this IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            var appConfig = app.ApplicationServices.GetRequiredService<AppConfig>();

            // Checking if the user exixts.
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