﻿using Microsoft.EntityFrameworkCore;
using Auth.Models;
using OpenIddict;
using OpenIddict.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Auth.Data
{
    public class ApplicationDbContext : OpenIddictContext<ApplicationUser, Application, IdentityRole , string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
