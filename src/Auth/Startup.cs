using Auth.Data;
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
                options.UseNpgsql(appConfig.DatabaseConfig.ConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add OpenIddict
            services.AddOpenIddict<ApplicationUser, ApplicationDbContext>()
                .Configure(options =>
                {
                    options.AllowInsecureHttp = HostingEnvironment.IsDevelopment();
                })
                .EnableTokenEndpoint("/auth/login")
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .UseJsonWebTokens()
                .AddEphemeralSigningKey(); // TODO: To be replaced with sign in certificate for production            

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

            app.UseOpenIddict();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = appConfig.AuthConfig.ClientId,
                Authority = appConfig.AuthConfig.Domain,
                AutomaticChallenge = true,
                AutomaticAuthenticate = true,
                RequireHttpsMetadata = env.IsProduction(),

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

            // Run all pending Migrations and Seed DB with initial data
            app.RunMigrationsAndSeedDb();
            app.UseStaticFiles();
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
    }
}
