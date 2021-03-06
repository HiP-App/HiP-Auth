﻿using Auth.Data;
using Auth.Models;
using Auth.Utility;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Threading;
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Auth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Read configurations from json
            var appConfig = new AppConfig(Configuration);

            // Register appConfig in Services 
            services.AddSingleton(appConfig);

            //Adding Cross Origin Requests 
            services.AddCors();

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(appConfig.DatabaseConfig.ConnectionString);
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Add OpenIddict
            services.AddOpenIddict(options =>
                {
                    options.DisableHttpsRequirement();
                    options.AddEntityFrameworkCoreStores<ApplicationDbContext>();
                    options.EnableTokenEndpoint("/auth/login");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.UseJsonWebTokens();
                    options.AddEphemeralSigningKey(); // TODO: To be replaced with sign in certificate for production  
                    options.AddMvcBinders();
                }
                );
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppConfig appConfig)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                // This will allow any request from any server. Tweak to fit your needs!
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            //app.UseOAuthValidation();
            app.UseOpenIddict();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = appConfig.AuthConfig.ClientId,
                Authority = appConfig.AuthConfig.Domain,
                AutomaticChallenge = true,
                AutomaticAuthenticate = true,
                //RequireHttpsMetadata = env.IsProduction(),
                RequireHttpsMetadata = false,
                Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        return Task.FromResult(0);
                    },

                    OnTokenValidated = context =>
                    {
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseMvc();
            app.UseMvcWithDefaultRoute();

            // Run all pending Migrations and Seed DB with initial data
            app.RunMigrationsAndSeedDb();
            app.UseStaticFiles();

            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();



            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();

            if (string.Equals("Development", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                host
                    .UseKestrel()
                    .UseUrls("http://0.0.0.0:5001");
            }
            else
            {
                // Only a hack, to get the service running https! Create a fake certificate for nginx
                var pwd = "password";
                var suppliers = new[] { "CN=localhost, OU=SupplierId, OU=HipScheme, O=OrgX, C=GB" };
                var cb = new X509CertBuilder(suppliers, "CN=HiP Admin, OU=hipcms, O=History in Paderborn, C=DE");
                var cert = cb.MakeCertificate(pwd, "CN=C001, OU=History in Paderborn, OU=HipScheme, O=History in Paderborn, C=DE", 2);

                File.WriteAllBytes("cert.pfx", cert.Export(X509ContentType.Pkcs12, pwd));
                host
                    .UseKestrel(cfg => cfg.UseHttps("cert.pfx", pwd))
                    .UseUrls("https://0.0.0.0:5001");
            }
            host.Build().Run();
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("console", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "console",
                        DisplayName = "My client application"
                    };

                    await manager.CreateAsync(application, "388D45FA-B36B-4988-BA59-B187D329C207", cancellationToken);
                }
            }
        }
    }
}
