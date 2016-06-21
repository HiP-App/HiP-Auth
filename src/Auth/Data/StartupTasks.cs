using Auth.Models;
using Auth.Models.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Auth.Data
{
    public class StartupTasks
    {
        public void CreateSuperuser(ApplicationDbContext context, IServiceProvider serviceProvider, IOptions<LoginViewModel> userCredentials)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Checking if the user exixts.
            var admin = userManager.FindByEmailAsync(userCredentials.Value.Email).Result;

            if (admin == null)
            {
                // Creating a new user and giving the user the Superuser role.
                admin = new ApplicationUser()
                {
                    UserName = userCredentials.Value.Email,
                    Email = userCredentials.Value.Email,
                };
                userManager.CreateAsync(admin, userCredentials.Value.Password);
            }

            // Assigning Superuser role if user admin already exists.
            if (!(userManager.IsInRoleAsync(admin, "Administrator").Result))
                userManager.AddToRoleAsync(admin, "Administrator");
        }
    }
}
